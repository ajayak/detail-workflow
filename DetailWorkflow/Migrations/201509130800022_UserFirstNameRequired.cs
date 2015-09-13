namespace DetailWorkflow.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserFirstNameRequired : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AspNetUsers", "FirstName", c => c.String(nullable: false, maxLength: 15));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AspNetUsers", "FirstName", c => c.String(maxLength: 15));
        }
    }
}
