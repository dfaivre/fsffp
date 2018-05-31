namespace FsDomainModeling

module CardGame =

    type Suit = Club | Diamond | Spade | Heart
    type Rank = Two | Three | Four | Five | Six | Seven | Eight
                | Nine | Ten | Jack | Queen | King | Ace
    type Card = Suit * Rank
    type Deck = Card list

    type Hand = Card list
    type Player = {
        Name:string
        Hand:Hand
    }

    type Game = {
        Deck:Deck
        Players: Player list
    }

    // Actions
    type Deal = Deck -> (Deck * Card)
    type PickupCard = (Hand * Card) -> Hand