namespace Project_OnlineBanking.Models;

public class Bieudo
{
    public int month { get; set; }

    public decimal AmountUp { get; set; }

    public decimal AmountDown { get; set;}

    public Bieudo(int month, decimal AmountUp, decimal AmountDown)
    {
        this.month = month;
        this.AmountUp = AmountUp;
        this.AmountDown = AmountDown;
    }

}
