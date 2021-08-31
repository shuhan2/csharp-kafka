// Copyright 2020 Confluent Inc.

// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at

// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using Confluent.Kafka;
using Confluent.Kafka.Admin;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Producer;


namespace CCloud
{
    class Program
    {
        static async Task<ClientConfig> LoadConfig(string configPath, string certDir)
        {
            try
            {
                var cloudConfig = (await File.ReadAllLinesAsync(configPath))
                    .Where(line => !line.StartsWith("#"))
                    .ToDictionary(
                        line => line.Substring(0, line.IndexOf('=')),
                        line => line.Substring(line.IndexOf('=') + 1));

                var clientConfig = new ClientConfig(cloudConfig);

                if (certDir != null)
                {
                    clientConfig.SslCaLocation = certDir;
                }

                return clientConfig;
            }
            catch (Exception e)
            {
                Console.WriteLine($"An error occured reading the config file from '{configPath}': {e.Message}");
                System.Environment.Exit(1);
                return null; // avoid not-all-paths-return-value compiler error.
            }
        }

        static async Task CreateTopicMaybe(string name, int numPartitions, short replicationFactor, ClientConfig cloudConfig)
        {
            using (var adminClient = new AdminClientBuilder(cloudConfig).Build())
            {
                try
                {
                    await adminClient.CreateTopicsAsync(new List<TopicSpecification> {
                        new TopicSpecification { Name = name, NumPartitions = numPartitions, ReplicationFactor = replicationFactor } });
                }
                catch (CreateTopicsException e)
                {
                    if (e.Results[0].Error.Code != ErrorCode.TopicAlreadyExists)
                    {
                        Console.WriteLine($"An error occured creating topic {name}: {e.Results[0].Error.Reason}");
                    }
                    else
                    {
                        Console.WriteLine("Topic already exists");
                    }
                }
            }
        }
        
        static void Produce(string topic, ClientConfig config)
        {
            var uom = new IdpProductUom()
            {
                Materialnumber = "000000006070107",
                Alternativeunitofmeasurement = "KLB",
                AlternativeUnitOfMeasurementShortText = "M3",
                Numeratorforconversiontobaseunitsofmeasure = "1.0",
                Denominatorforconversiontobaseunitsofmeasure = "3.0",
                LastUpdated = "2021-08-19T12:11:00",
                ACTION = "U"
            };
            var uom2 = new IdpProductUom()
            {
                Materialnumber = "000000006070107",
                Alternativeunitofmeasurement = "M3",
                AlternativeUnitOfMeasurementShortText = "M3",
                Numeratorforconversiontobaseunitsofmeasure = "2.0",
                Denominatorforconversiontobaseunitsofmeasure = "3.0",
                LastUpdated = "2021-08-20T19:11:00",
                ACTION = "U"
            };
            var s = new[] {uom, uom2};
            var idpProductSalesText = new IdpProductSalesText()
            {
                MaterialNumber = "000000000005000003",
                SalesOrganization = "1796",
                SalesOrganizationName = "ZXMO",
                FlagMaterialDeletion = "false",
                MaterialSalesText = "ESCZ5600\n",
                Language = "E",
                LastUpdated = "2021-06-18T03:31:10",
                ACTION = "U"
            };
            
            
            var idpProductSalesText2 = new IdpProductSalesText()
            {
                MaterialNumber = "000000000005000003",
                SalesOrganization = "1796",
                SalesOrganizationName = "ZX12",
                FlagMaterialDeletion = "false",
                MaterialSalesText = "ESCZ5600\n",
                Language = "E",
                LastUpdated = "2021-08-18T03:31:10",
                ACTION = "U"
            };

            var sales = new[]
            {
                idpProductSalesText,
                idpProductSalesText2
            };
            
            var idpCustomer = new IdpCustomer()
            {
                Customer = "002",
                Name = "01",
                Name2 = "02",
                DeletionFlag = "1",
                CentralOrderBlock = "01",
                AccountGroup = "0003",
                AccountGroupName = "44",
                CountryGroupingCode = "55",
                AddressNumber = "006",
                LastUpdated = "2021-08-01T16:20:20",
                ACTION = "U"
            };
            
            
            var idpCustomer2 = new IdpCustomer()
            {
                Customer = "002",
                Name = "01",
                Name2 = "02",
                DeletionFlag = "1",
                CentralOrderBlock = "01",
                AccountGroup = "0004",
                AccountGroupName = "44",
                CountryGroupingCode = "55",
                AddressNumber = "006",
                LastUpdated = "2021-08-02T16:20:20",
                ACTION = "U"
            };
            var customers = new[] {idpCustomer, idpCustomer2};
            
             var idpContract = new IdpContract()
            {
                SalesDocument = "0043463653",
                SalesDocumentType = "OR",
                SalesDocumentTypeDescription = "Standard Order",
                CreatedDate = "2021-06-02T00:00:00",
                ShipToParty = "0127430001",
                SalesOrganization = "1796",
                DistributionChannel = "10",
                Division = "31",
                ShippingConditions = "11",
                ShippingConditionsDescription = "Container",
                ShippingConditionsDescriptionZh = "集装箱海运",
                ModeOfTransport = "10",
                ContractProposalValidFrom = "0101-01-01T00:00:00",
                QuotationValidFrom = "0101-01-01T00:00:00",
                ContractProposalValidTo = "2022-01-01T00:00:00",
                QuotationValidTo = "2021-01-01T00:00:00",
                CreditChecksOverallStatus = "A",
                DeliveryBlock = string.Empty,
                NetValue = "120000.00",
                DocumentCurrency = "USD",
                TermsOfPaymentKey = "O030",
                Incoterms = "CIF",
                Incoterms2 = "NHAVA SHEVA1",
                UnloadingPoint = "NHAVA SHEVA",
                DeliveryInstruction = "MODE OF SHIPMENT : 2 X 42G0 FCL\n",
                ReleasedForOrderCreation = true,
                CustomerPurchaseOrderNumber = "ZKB 095 ysdr84 01",
                CustomerPurchaseOrderDate = "2021-06-02T00:00:00",
                PaymentGuaranteeProcedure = "000020",
                SalesOrganizationDescription = "EM Asia Pac (EMAPPL)",
                PaymentTermDescription = "30 days from B/L date",
                PaymentGuaranteeProcedureDescription = "Documentary Letters of Credit",
                SDDocumentCategory = "C",
                LastUpdated = "2021-08-24T21:59:26",
                SalesItem = "[{\"ITEM_ACTION\":\"U\",\"Item_Number\":\"000010\",\"Requested_Delivery_Date\":\"0101-01-01\",\"Material_Number\":\"000000000005054544\",\"Material_Description\":\"EXXSOL D80 AP VL\",\"Sales_Unit\":\"MT\",\"Net_Price\":0.00,\"Order_Quantity_In_Sales_Units\":0.000,\"Target_Quantity_In_Sales_Units\":2000.000,\"Plant\":\"\",\"Customer_Material_Number\":\"\",\"Material_Type\":\"ZXMO\",\"Product_Remaining_Quantity\":1.000,\"Material_Sales_Text\":\"EXXSOL D80 FLUID\\n\",\"Reason_For_Rejection\":\"\",\"Product_Total_Quantity\":2000.000}]",
                ACTION = "U"
            };
             
             var contracts = new[] {idpContract};
            
            using (var producer = new ProducerBuilder<string, string>(config).Build())
            {
                int numProduced = 0;
                int numMessages = 2;
                for (int i=0; i< 1; ++i)
                {
                    var key = "";
                    var val = JObject.FromObject(s[i]).ToString(Formatting.None);;
                    switch (topic)
                    {
                        case "topical-materialauom": 
                            val = JObject.FromObject(s[i]).ToString(Formatting.None);
                            break;
                        case "topic-gen-materialsalestext": 
                            val = JObject.FromObject(sales[i]).ToString(Formatting.None);
                            break;
                        case "topic-customermaster": 
                            val = JObject.FromObject(customers[i]).ToString(Formatting.None);
                            break;
                        
                        case "topic-gen-contractquotation": 
                            val = JObject.FromObject(contracts[i]).ToString(Formatting.None);
                            break;
                    }

                    Console.WriteLine($"Producing record: {key} {val}");

                    producer.Produce(topic, new Message<string, string> { Key = null, Value = val },
                        (deliveryReport) =>
                        {
                            if (deliveryReport.Error.Code != ErrorCode.NoError)
                            {
                                Console.WriteLine($"Failed to deliver message: {deliveryReport.Error.Reason}");
                            }
                            else
                            {
                                Console.WriteLine($"Produced message to: {deliveryReport.TopicPartitionOffset}");
                                numProduced += 1;
                            }
                        });
                }

                producer.Flush(TimeSpan.FromSeconds(10));

                Console.WriteLine($"{numProduced} messages were produced to topic {topic}");
            }
        }

        static async Task Main(string[] args)
        {
            // if (args.Length != 3 && args.Length != 4) { PrintUsage(); }
            
            // var mode = args[0];
            // var topic = "topic-customermaster";
            // var topic = "topic-gen-materialsalestext";
            // var topic = "topic-materialauom";
            var topic = "topic-gen-contractquotation";
            
            // var configPath = args[2];
            // var certDir = args.Length == 4 ? args[3] : null;

            var config = new ClientConfig()
            {
                BootstrapServers = "localhost:9092"

            };
            Produce(topic, config);
            // switch (mode)
            // {
            //     case "produce":
            //         // await CreateTopicMaybe(topic, 1, 3, config);
            //         Produce(topic, config);
            //         break;
            //     case "consume":
            //         Consume(topic, config);
            //         break;
            //     default:
            //         PrintUsage();
            //         break;
            // }
        }
    }
}
