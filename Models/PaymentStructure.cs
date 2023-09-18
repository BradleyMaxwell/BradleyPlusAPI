namespace api.Models
{
	public class PaymentStructure
	{
		Guid Id { get; }
		float CostPerBilling { get; }
		int MonthsPerBilling { get; }

		public PaymentStructure(Guid id, float costPerBilling, int monthsPerBilling)
		{
			Id = id;
			CostPerBilling = costPerBilling;
			MonthsPerBilling = monthsPerBilling;
		}
	}
}

