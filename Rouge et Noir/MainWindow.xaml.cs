﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Model;
using View;

namespace Rouge_et_Noir {

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {

        private GameTable table;

        private TableauView[] tableauViews;
        

        public MainWindow() {
            InitializeComponent();

            table = new GameTable();
            table.NewGame();

            tableauViews = new TableauView[GameTable.TABLEAUS];
            SetTableau();
            SetFoundations();

            CardView cardView = new CardView();
            cardView.SetCard(table.GetTableau(3).GetTopCard());
            Grid.SetColumn(cardView, 0);
            Grid.SetRow(cardView, 0);
            rootView.Children.Add(cardView);

            /*Deck deck = new Deck104();
            
            LeftFoundation left = new LeftFoundation();
            for (int i = 0; i <= 1; i++) {
                foreach (Card card in deck) {
                    left.AddCard(card);
                }
                System.Diagnostics.Debug.Print(left.ToString());
                System.Diagnostics.Debug.Print("-------------------\n\n\n");
            }*/

        }

        private void SetTableau() {
            for (int i = 0; i < GameTable.TABLEAUS; i++) {
                var tableauView = new TableauView();

                Grid.SetColumn(tableauView, i);
                Grid.SetRow(tableauView, 1);
                tableauView.SetTableau(table.GetTableau(i));

                rootView.Children.Add(tableauView);
            }
        }

        private void SetFoundations() {
            for (int i = 0; i < GameTable.FOUNDATIONS * 2; i++) {
                var foundationView = new FoundationView();

                int num = i % GameTable.FOUNDATIONS;
                bool left = i < GameTable.FOUNDATIONS;

                Grid.SetColumn(foundationView, 2 + i);
                Grid.SetRow(foundationView, 0);
                foundationView.SetFoundation(table.GetFoundation(num, left), left);

                rootView.Children.Add(foundationView);
            }
        }
    }
}
