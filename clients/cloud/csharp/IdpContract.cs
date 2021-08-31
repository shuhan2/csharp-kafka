namespace CCloud
{

    public class IdpContract 
    {
        public string SalesDocument { get; set; }
        
        public string SalesDocumentType { get; set; }

        public string SalesDocumentTypeDescription { get; set; }

        public string SDDocumentCategory { get; set; }

        public string ShipToParty { get; set; }

        public string SalesOrganization { get; set; }

        public string SalesOrganizationDescription { get; set; }

        public string DistributionChannel { get; set; }

        public string Division { get; set; }

        public string ShippingConditions { get; set; }

        public string ShippingConditionsDescription { get; set; }

        public string ShippingConditionsDescriptionZh { get; set; }

        public string ModeOfTransport { get; set; }

        public string CreatedDate { get; set; }

        public string QuotationValidFrom { get; set; }
        
        public string ContractProposalValidFrom { get; set; }
        
        public string QuotationValidTo { get; set; }
        
        public string ContractProposalValidTo { get; set; }
        
        public string CreditChecksOverallStatus { get; set; }
        
        public string DeliveryBlock { get; set; }
        
        public string NetValue { get; set; }
        
        public string DocumentCurrency { get; set; }
        
        public string CustomerPurchaseOrderNumber { get; set; }
        
        public string CustomerPurchaseOrderDate { get; set; }
        
        public string TermsOfPaymentKey { get; set; }
        
        public string PaymentTermDescription { get; set; }
        
        public string Incoterms { get; set; }
        
        public string Incoterms2 { get; set; }
        
        public string UnloadingPoint { get; set; }
        
        public string PaymentGuaranteeProcedure { get; set; }
        
        public string PaymentGuaranteeProcedureDescription { get; set; }
        
        public string DeliveryInstruction { get; set; }
        
        public string GeneralText { get; set; }
        
        public string DocumentationText { get; set; }
        
        public bool ReleasedForOrderCreation { get; set; }
        
        public string LastUpdated { get; set; }
        
        public string SalesItem { get; set; }
       
        public string ACTION { get; set; }

        public class IdpContractItem
        {
            public string Item_Action { get; set; }
            
            public string Item_Number { get; set; }
            
            public string Material_Number { get; set; }
            
            public string Material_Description { get; set; }
            
            public string Sales_Unit { get; set; }
            
            public decimal? Product_Total_Quantity { get; set; }
            
            public decimal? Product_Remaining_Quantity { get; set; }
            
            public string Plant { get; set; }
            
            public string Customer_Material_Number { get; set; }
            
            public string Material_Sales_Text { get; set; }
            
            public string Reason_For_Rejection { get; set; }
            
            public string Reason_For_Rejection_Description { get; set; }
        }
        
    }
}