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
                        QuestionKeywordId = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.QuestionId)
                .ForeignKey("dbo.QuestionGroups", t => t.QuestionGroupId, cascadeDelete: true)
                .ForeignKey("dbo.QuestionKeywords", t => t.QuestionKeywordId, cascadeDelete: true)
                .Index(t => t.QuestionGroupId)
                .Index(t => t.QuestionKeywordId);
            
            CreateTable(
                "dbo.Messages",
                c => new
                    {
                        MessageId = c.Int(nullable: false, identity: true),
                        QuestionId = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                        MessageText = c.String(nullable: false),
                        Date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.MessageId)
                .ForeignKey("dbo.UserProfile", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.Questions", t => t.QuestionId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.QuestionId);
            
            CreateTable(
                "dbo.UserProfile",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        UserName = c.String(),
                    })
                .PrimaryKey(t => t.UserId);
            
            CreateTable(
                "dbo.Votes",
                c => new
                    {
                        VoteId = c.Int(nullable: false, identity: true),
                        QuestionId = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.VoteId)
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
            
            CreateTable(
                "dbo.QuestionKeywords",
                c => new
                    {
                        QuestionKeywordId = c.Int(nullable: false, identity: true),
                        QuestionKeywordName = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.QuestionKeywordId);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.Votes", new[] { "QuestionId" });
            DropIndex("dbo.Messages", new[] { "QuestionId" });
            DropIndex("dbo.Messages", new[] { "UserId" });
            DropIndex("dbo.Questions", new[] { "QuestionKeywordId" });
            DropIndex("dbo.Questions", new[] { "QuestionGroupId" });
            DropForeignKey("dbo.Votes", "QuestionId", "dbo.Questions");
            DropForeignKey("dbo.Messages", "QuestionId", "dbo.Questions");
            DropForeignKey("dbo.Messages", "UserId", "dbo.UserProfile");
            DropForeignKey("dbo.Questions", "QuestionKeywordId", "dbo.QuestionKeywords");
            DropForeignKey("dbo.Questions", "QuestionGroupId", "dbo.QuestionGroups");
            DropTable("dbo.QuestionKeywords");
            DropTable("dbo.QuestionGroups");
            DropTable("dbo.Votes");
            DropTable("dbo.UserProfile");
            DropTable("dbo.Messages");
            DropTable("dbo.Questions");
        }
    }
}
