using Microsoft.EntityFrameworkCore.Migrations;

namespace PomeloHealthApi.Migrations
{
    public partial class AddTriggers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("CREATE TRIGGER `clinics_insert_trigger` BEFORE INSERT ON `Clinics` FOR EACH ROW SET NEW.CreatedAt = NOW(), NEW.UpdatedAt = NOW();");
            migrationBuilder.Sql("CREATE TRIGGER `clinics_update_trigger` BEFORE UPDATE ON `Clinics` FOR EACH ROW SET NEW.CreatedAt = NOW(), NEW.UpdatedAt = NOW();");

            migrationBuilder.Sql("CREATE TRIGGER `patients_insert_trigger` BEFORE INSERT ON `Patients` FOR EACH ROW SET NEW.CreatedAt = NOW(), NEW.UpdatedAt = NOW();");
            migrationBuilder.Sql("CREATE TRIGGER `patients_update_trigger` BEFORE UPDATE ON `Patients` FOR EACH ROW SET NEW.CreatedAt = NOW(), NEW.UpdatedAt = NOW(); ");

            migrationBuilder.Sql("CREATE TRIGGER `providers_insert_trigger` BEFORE INSERT ON `Providers` FOR EACH ROW SET NEW.CreatedAt = NOW(), NEW.UpdatedAt = NOW(); ");
            migrationBuilder.Sql("CREATE TRIGGER `providers_update_trigger` BEFORE UPDATE ON `Providers` FOR EACH ROW SET NEW.CreatedAt = NOW(), NEW.UpdatedAt = NOW(); ");

            migrationBuilder.Sql("CREATE TRIGGER `availabilities_insert_trigger` BEFORE INSERT ON `Availabilities` FOR EACH ROW SET NEW.CreatedAt = NOW(), NEW.UpdatedAt = NOW();");
            migrationBuilder.Sql("CREATE TRIGGER `availabilities_update_trigger` BEFORE UPDATE ON `Availabilities` FOR EACH ROW SET NEW.CreatedAt = NOW(), NEW.UpdatedAt = NOW();");

            migrationBuilder.Sql("CREATE TRIGGER `appointments_insert_trigger` BEFORE INSERT ON `Appointments` FOR EACH ROW SET NEW.CreatedAt = NOW(), NEW.UpdatedAt = NOW();");
            migrationBuilder.Sql("CREATE TRIGGER `appointments_update_trigger` BEFORE UPDATE ON `Appointments` FOR EACH ROW SET NEW.CreatedAt = NOW(), NEW.UpdatedAt = NOW(); ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP TRIGGER IF EXISTS clinics_insert_trigger");
            migrationBuilder.Sql("DROP TRIGGER IF EXISTS clinics_update_trigger");
            migrationBuilder.Sql("DROP TRIGGER IF EXISTS patients_insert_trigger");
            migrationBuilder.Sql("DROP TRIGGER IF EXISTS providers_insert_trigger");
            migrationBuilder.Sql("DROP TRIGGER IF EXISTS providers_update_trigger");
            migrationBuilder.Sql("DROP TRIGGER IF EXISTS availabilities_insert_trigger");
            migrationBuilder.Sql("DROP TRIGGER IF EXISTS availabilities_update_trigger");
            migrationBuilder.Sql("DROP TRIGGER IF EXISTS appointments_insert_trigger");
            migrationBuilder.Sql("DROP TRIGGER IF EXISTS appointments_update_trigger");
        }
    }
}
