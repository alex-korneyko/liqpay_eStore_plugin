using AlexApps.Plugin.Payment.LiqPay.Domain;
using FluentMigrator;
using Nop.Data.Migrations;

namespace AlexApps.Plugin.Payment.LiqPay.Data.Migrations
{
    [NopMigration("2021/11/13 14:00:00:0000000", "Create customer cardToken table")]
    public class InitialMigration : AutoReversingMigration
    {
        private readonly IMigrationManager _migrationManager;

        public InitialMigration(IMigrationManager migrationManager)
        {
            _migrationManager = migrationManager;
        }

        public override void Up()
        {
            _migrationManager.BuildTable<LiqPayOneClickCustomerCardToken>(Create);
        }
    }
}