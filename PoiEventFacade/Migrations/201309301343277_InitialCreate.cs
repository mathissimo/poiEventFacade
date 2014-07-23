using System.Diagnostics.CodeAnalysis;

namespace PoiEventNetwork.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    [ExcludeFromCodeCoverage]
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Tags",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Text = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateIndex("dbo.Tags", "Text", unique: true);

            CreateTable(
                "dbo.AbsTagAbles",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        Street = c.String(),
                        City = c.String(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                        Location_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Coordinates", t => t.Location_Id)
                .Index(t => t.Location_Id);
            
            CreateTable(
                "dbo.Coordinates",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Lon = c.Single(nullable: false),
                        Lat = c.Single(nullable: false),
                        NgonPoi_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AbsTagAbles", t => t.NgonPoi_Id)
                .Index(t => t.NgonPoi_Id);
            
            CreateTable(
                "dbo.Events",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        Desc = c.String(),
                        Date = c.DateTime(nullable: false),
                        CreatorId = c.Long(nullable: false),
                        Location_Id = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AbsTagAbles", t => t.Location_Id)
                .ForeignKey("dbo.Users", t => t.CreatorId)
                .Index(t => t.Location_Id)
                .Index(t => t.CreatorId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Mail = c.String(),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Rights = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Messages",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        Text = c.String(),
                        User_Id = c.Long(),
                        Event_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.User_Id)
                .ForeignKey("dbo.Events", t => t.Event_Id)
                .Index(t => t.User_Id)
                .Index(t => t.Event_Id);
            
            CreateTable(
                "dbo.ObjToTag",
                c => new
                    {
                        ObjId = c.Long(nullable: false),
                        TagId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => new { t.ObjId, t.TagId })
                .ForeignKey("dbo.AbsTagAbles", t => t.ObjId, cascadeDelete: true)
                .ForeignKey("dbo.Tags", t => t.TagId, cascadeDelete: true)
                .Index(t => t.ObjId)
                .Index(t => t.TagId);
            
            CreateTable(
                "dbo.EvtToUsr",
                c => new
                    {
                        EvtId = c.Long(nullable: false),
                        UsrId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => new { t.EvtId, t.UsrId })
                .ForeignKey("dbo.Events", t => t.EvtId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UsrId, cascadeDelete: true)
                .Index(t => t.EvtId)
                .Index(t => t.UsrId);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.EvtToUsr", new[] { "UsrId" });
            DropIndex("dbo.EvtToUsr", new[] { "EvtId" });
            DropIndex("dbo.ObjToTag", new[] { "TagId" });
            DropIndex("dbo.ObjToTag", new[] { "ObjId" });
            DropIndex("dbo.Messages", new[] { "Event_Id" });
            DropIndex("dbo.Messages", new[] { "User_Id" });
            DropIndex("dbo.Events", new[] { "CreatorId" });
            DropIndex("dbo.Events", new[] { "Location_Id" });
            DropIndex("dbo.Coordinates", new[] { "NgonPoi_Id" });
            DropIndex("dbo.AbsTagAbles", new[] { "Location_Id" });
            DropForeignKey("dbo.EvtToUsr", "UsrId", "dbo.Users");
            DropForeignKey("dbo.EvtToUsr", "EvtId", "dbo.Events");
            DropForeignKey("dbo.ObjToTag", "TagId", "dbo.Tags");
            DropForeignKey("dbo.ObjToTag", "ObjId", "dbo.AbsTagAbles");
            DropForeignKey("dbo.Messages", "Event_Id", "dbo.Events");
            DropForeignKey("dbo.Messages", "User_Id", "dbo.Users");
            DropForeignKey("dbo.Events", "CreatorId", "dbo.Users");
            DropForeignKey("dbo.Events", "Location_Id", "dbo.AbsTagAbles");
            DropForeignKey("dbo.Coordinates", "NgonPoi_Id", "dbo.AbsTagAbles");
            DropForeignKey("dbo.AbsTagAbles", "Location_Id", "dbo.Coordinates");
            DropTable("dbo.EvtToUsr");
            DropTable("dbo.ObjToTag");
            DropTable("dbo.Messages");
            DropTable("dbo.Users");
            DropTable("dbo.Events");
            DropTable("dbo.Coordinates");
            DropTable("dbo.AbsTagAbles");
            DropTable("dbo.Tags");
        }
    }
}
