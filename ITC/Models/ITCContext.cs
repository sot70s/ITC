using System.Data.Entity;

namespace ITC.Models
{
    public class ITCContext : DbContext
    {
        public DbSet<JobReqHeader> JobReqHeader { get; set; }
        public DbSet<JobReqBody> JobReqBody { get; set; }
        public DbSet<HRM_Employee> HRM_Employee { get; set; }
        public DbSet<HRM_Employee_Manager> HRM_Employee_Manager { get; set; }
        public DbSet<RolePage> RolePage { get; set; }
        public DbSet<RoleUser> RoleUser { get; set; }
        public DbSet<RoleCoordinator> RoleCoordinator { get; set; }
        public DbSet<Software> Software { get; set; }
        public DbSet<SoftwareFile> SoftwareFile { get; set; }
        public DbSet<SoftwareHolder> SoftwareHolder { get; set; }
        public DbSet<SoftwareTemplate> SoftwareTemplate { get; set; }
        public DbSet<Approver> Approver { get; set; }
        public DbSet<MisFlow> MisFlow { get; set; }
        public DbSet<Symptom> Symptom { get; set; }
        public DbSet<Grade> Grade { get; set; }
        public DbSet<Priority> Priority { get; set; }
        public DbSet<GroupCoordinator> GroupCoordinator { get; set; }
        public DbSet<RejectJobApprover> RejectJobApprover { get; set; }
        public DbSet<JobApprove> JobApprove { get; set; }
        public DbSet<SolutionSuggestion> SolutionSuggestion { get; set; }
        public DbSet<WorkOrders> WorkOrders { get; set; }
        public DbSet<JobPlanning> JobPlanning { get; set; }
        public DbSet<JobDaily> JobDaily { get; set; }
        public DbSet<JobRework> JobRework { get; set; }
        public DbSet<JobUnAccept> JobUnAccept { get; set; }
        public DbSet<HardwareAsset> HardwareAssets { get; set; }
        public DbSet<Equipment_Type> Equipment_Types { get; set; }
        public DbSet<JobComment> JobComment { get; set; }
        public DbSet<JobReply> JobReply { get; set; }
        public DbSet<SOP> SOP { get; set; }
        public DbSet<SOPTemplate> SOPTemplate { get; set; }
        public DbSet<SOPAsset> SOPAsset { get; set; }
        public DbSet<RoleSR> RoleSR { get; set; }
        public DbSet<Inventory> Inventory { get; set; }
        public DbSet<InventoryType> InventoryType { get; set; }
        public DbSet<Lot> Lot { get; set; }
        public DbSet<SparesCode> SparesCode { get; set; }
        public DbSet<SparesCodeTemplate> SparesCodeTemplate { get; set; }
        public DbSet<PartAsset> PartAsset { get; set; }
        public DbSet<SystemRequestHeader> SystemRequestHeader { get; set; }
    }
}