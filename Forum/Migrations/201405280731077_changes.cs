namespace Forum.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changes : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Questions", "Handled");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Questions", "Handled", c => c.String());
        }
    }
}
