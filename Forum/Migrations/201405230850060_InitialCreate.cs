namespace Forum.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Questions",
                c => new
                    {
                        QuestionId = c.Int(nullable: false, identity: true),
                        QuestionGroupId = c.Int(nullable: false),
                        QuestionName = c.String(nullable: false),
                        QuestionDescription = c.String(nullable: false),
                        QuestionKeyword = c.String(nullable: false),
                        DateTime = c.DateTime(nullable: false),
                        Votes = c.String(),
                        Handled = c.String(),
                    })
                .PrimaryKey(t => t.QuestionId)
                .ForeignKey("dbo.QuestionGroups", t => t.QuestionGroupId, cascadeDelete: true)
                .Index(t => t.QuestionGroupId);
            
            CreateTable(
                "dbo.Messages",
                c => new
                    {
                        MessageId = c.Int(nullable: false, identity: true),
                        QuestionId = c.Int(nullable: false),
                        MessageText = c.String(nullable: false),
                        DateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.MessageId)
                .ForeignKey("dbo.Questions", t => t.QuestionId, cascadeDelete: true)
                .Index(t => t.QuestionId);
            
            CreateTable(
                "dbo.QuestionGroups",
                c => new
                    {
                        QuestionGroupId = c.Int(nullable: false, identity: true),
                        QuestionGroupName = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.QuestionGroupId);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.Messages", new[] { "QuestionId" });
            DropIndex("dbo.Questions", new[] { "QuestionGroupId" });
            DropForeignKey("dbo.Messages", "QuestionId", "dbo.Questions");
            DropForeignKey("dbo.Questions", "QuestionGroupId", "dbo.QuestionGroups");
            DropTable("dbo.QuestionGroups");
            DropTable("dbo.Messages");
            DropTable("dbo.Questions");
        }
    }
}
