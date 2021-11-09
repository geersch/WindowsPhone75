using System;
using System.Collections.Generic;
using Microsoft.Phone.Controls;
using StackExchange.Api;

namespace StackOverflow.Phone.UI
{
    public partial class MainPage : PhoneApplicationPage
    {
        private StackExchangeApi _api;

        // Constructor
        public MainPage()
        {
            InitializeComponent();

            this._api = new StackExchangeApi("your api key");

            Loaded += MainPage_Loaded;
        }

        void MainPage_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            refreshButton_Click(null, null);
        }

        void UserRetrieved(User user)
        {
            Dispatcher.BeginInvoke(() =>
                                       {
                                           DisplayName.Text = user.DisplayName;
                                           Reputation.Text = user.Reputation.ToString();
                                           LastSeen.Text = user.LastAccessDate.ToString();
                                           Website.Text = user.Website;
                                           Location.Text = user.Location;
                                           Age.Text = user.Age.ToString();
                                           Views.Text = user.Views.ToString();
                                           BronzeBadges.Text = user.BadgeCounts.Bronze.ToString();
                                           SilverBadges.Text = user.BadgeCounts.Silver.ToString();
                                           GoldBadges.Text = user.BadgeCounts.Gold.ToString();
                                       });
        }

        void ReputationRetrieved(List<ReputationChange> reputation)
        {
            Dispatcher.BeginInvoke(() =>
                                       {
                                           ReputationList.ItemsSource = reputation;
                                       });
        }

        void settingsButton_Click(object sender, EventArgs e)
        {
        }

        void refreshButton_Click(object sender, EventArgs e)
        {
            ProgressIndicator.IsVisible = true;
            var callback = new StackExchangeApi.StackExchangeApiCallback<User>(UserRetrieved);
            this._api.GetUser(893099, callback);

            var reputationCallback =
                new StackExchangeApi.StackExchangeApiCallback<List<ReputationChange>>(ReputationRetrieved);
            this._api.GetReputationChanges(893099, DateTime.Now.AddDays(-100), DateTime.Now, reputationCallback);
        }
    }
}