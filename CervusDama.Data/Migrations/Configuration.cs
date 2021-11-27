namespace CervusDama.Data.Migrations
{
	using System.Collections.Generic;
	using System.Data.Entity.Migrations;

	internal sealed class Configuration : DbMigrationsConfiguration<CervusDama.Data.CervusDamaContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(CervusDama.Data.CervusDamaContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data.

            /*IList<Entities.Role> defaultRoles = new List<Entities.Role>();
            defaultRoles.Add(new Entities.Role() { Name = "Administrator" });
            defaultRoles.Add(new Entities.Role() { Name = "Moderator" });
            defaultRoles.Add(new Entities.Role() { Name = "Editor" });
            defaultRoles.Add(new Entities.Role() { Name = "Yazar" });

            context.Role.AddRange(defaultRoles);

            base.Seed(context);*/
        }
    }
}
