using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace iCustomer.Models
{
    public class Customer : INotifyPropertyChanged
    {
        private static readonly Random _rng = new Random();

        // Multipliers per month so expenses follow a realistic seasonal pattern.
        // Higher in autumn (Sep–Nov), lower in winter (Jan–Feb).
        private static readonly double[] SeasonalWeights =
        {
            0.60, 0.65, 0.90, 1.00, 1.10, 1.15,
            0.80, 0.75, 1.20, 1.30, 1.35, 0.85
        };

        private int _id;
        private string _dni;
        private string _name;
        private string _lastName;
        private string? _email;
        private string? _phone;
        private DateTime _dataAlta;

        public int Id
        {
            get => _id;
            set { _id = value; OnPropertyChanged(); }
        }

        public string Dni
        {
            get => _dni;
            set { _dni = value; OnPropertyChanged(); }
        }

        public string Name
        {
            get => _name;
            set { _name = value; OnPropertyChanged(); }
        }

        public string LastName
        {
            get => _lastName;
            set { _lastName = value; OnPropertyChanged(); }
        }

        public string? Email
        {
            get => _email;
            set { _email = value; OnPropertyChanged(); }
        }

        public string? Phone
        {
            get => _phone;
            set { _phone = value; OnPropertyChanged(); }
        }

        public DateTime DataAlta
        {
            get => _dataAlta;
            set { _dataAlta = value; OnPropertyChanged(); }
        }

        // Monthly expenses Jan–Dec. Generated once on construction and
        // kept for the whole session so the chart always shows the same data.
        public double[] MonthlyExpenses { get; }

        public Customer(int id_entrada, string dni_entrada, string name_entrada,
                        string lastname_entrada, string? email_entrada,
                        string phone_entrada, DateTime dataAlta_entrada)
        {
            Id = id_entrada;
            Dni = dni_entrada;
            Name = name_entrada;
            LastName = lastname_entrada;
            Email = email_entrada;
            Phone = phone_entrada;
            DataAlta = dataAlta_entrada;

            MonthlyExpenses = GenerateMonthlyExpenses();
        }

        // Picks a random base spend (500–5000 €) and scales it by each
        // month's seasonal weight to get 12 realistic monthly values.
        private static double[] GenerateMonthlyExpenses()
        {
            double baseMontly = 500 + _rng.NextDouble() * 4_500;

            var expenses = new double[12];
            for (int i = 0; i < 12; i++)
                expenses[i] = Math.Round(baseMontly * SeasonalWeights[i], 2);

            return expenses;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}