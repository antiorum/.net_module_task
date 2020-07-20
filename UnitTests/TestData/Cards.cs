using System.Collections.Generic;
using DataService.Models;

namespace UnitTests.TestData
{
    public class Cards
    {
        public  Card CardOne { get; } = new Card
        {
            Id = 1,
            Name = "One",
            Value = 1,
            Image = "1.jpg",
            Owner = null,
            Decks = new HashSet<Deck>()
        };
        public  Card CardFive { get; } = new Card
        {
            Id = 2,
            Name = "Five",
            Value = 5,
            Image = "5.jpg",
            Owner = null,
            Decks = new HashSet<Deck>()
        };
        public  Card CardSeven { get; } = new Card
        {
            Id = 3,
            Name = "Seven",
            Value = 7,
            Image = "7.jpg",
            Owner = null,
            Decks = new HashSet<Deck>() 
        };

        public  Card CardCoffee { get; } = new Card
        {
            Id = 4,
            Name = "Coffee",
            Value = null,
            Image = "coffee.jpg",
            Owner = null,
            Decks = new HashSet<Deck>() 
        };

        public  Card CardEight { get; } = new Card
        {
            Id = 5,
            Name = "Eight",
            Value = 8,
            Image = "eight.jpg",
            Owner = "Bor'ka",
            Decks = new HashSet<Deck>() 
        };

        public  Card CardTen { get; } = new Card
        {
            Id = 6,
            Name = "Ten",
            Value = 8,
            Image = "eight.jpg",
            Owner = "Bor'ka",
            Decks = new HashSet<Deck>() 
        };

        public Card CardTwo { get; } = new Card
        {
          Id = 7,
          Name = "Two",
          Value = 2,
          Image = "two.jpg",
          Owner = null,
          Decks = null
        };
  }
}
