namespace NuGetGallery.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTriggerForPackagesLastEdited : DbMigration
    {
        public override void Up()
        {
            // If the "LastEdited" field of a package was edited, update it to the current timestamp.
            // This guarantees that all "LastEdited" timestamps were generated by the same source: the gallery database.
            //
            // Multiple instances of the gallery running simultaneously will have slightly different local machine times.
            // Therefore, one source must generate all "LastEdited" timestamps because otherwise there will be slight discrepancies in "LastEdited" which can lead to packages being inserted out of order into the feed.
            //
            // Note that UPDATE(LastEdited) is true when a row is inserted for the first time, so we must add a INSERTED.LastEdited IS NOT NULL check to the UPDATE statement.

            Sql(@"
CREATE TRIGGER [dbo].[LastEditedTrigger] ON [dbo].[Packages]
AFTER INSERT, UPDATE
AS
BEGIN
    IF (UPDATE(LastEdited))
    BEGIN
        UPDATE [dbo].[Packages]
        SET LastEdited = GETUTCDATE()
        FROM INSERTED
        WHERE [dbo].[Packages].[Key] = INSERTED.[Key] AND INSERTED.LastEdited IS NOT NULL
    END
END");
        }

        public override void Down()
        {
            Sql(@"DROP TRIGGER [dbo].[LastEditedTrigger]");
        }
    }
}
