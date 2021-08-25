namespace MediaOrganiser1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initials : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FileClasses",
                c => new
                    {
                        FileClassId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        File = c.Binary(),
                        ContentType = c.String(),
                        Category = c.String(),
                        Genre = c.String(),
                    })
                .PrimaryKey(t => t.FileClassId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.FileClasses");
        }
    }
}
