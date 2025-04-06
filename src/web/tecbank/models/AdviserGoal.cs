namespace tecbank.models{
    /// <summary>
    /// Model class for goals set for an adviser
    /// </summary>
    /// adviser_id, target_profit, start_date, limit_date, state, currency_id
    public class AdviserGoal {
        public int adviser_id {get; set;}
        public int target_profit {get; set;}
        public DateOnly start_date {get; set;}
        public DateOnly limit_date {get; set;}
        public int state {get; set;}
        public int currency_id {get; set;}
    }
}