using Abp.Authorization;
using Abp.Configuration.Startup;
using Abp.Localization;
using Abp.MultiTenancy;

namespace Arch.Authorization
{
    /// <summary>
    /// Application's authorization provider.
    /// Defines permissions for the application.
    /// See <see cref="AppPermissions"/> for all permission names.
    /// </summary>
    public class AppAuthorizationProvider : AuthorizationProvider
    {
        private readonly bool _isMultiTenancyEnabled;

        public AppAuthorizationProvider(bool isMultiTenancyEnabled)
        {
            _isMultiTenancyEnabled = isMultiTenancyEnabled;
        }

        public AppAuthorizationProvider(IMultiTenancyConfig multiTenancyConfig)
        {
            _isMultiTenancyEnabled = multiTenancyConfig.IsEnabled;
        }

        public override void SetPermissions(IPermissionDefinitionContext context)
        {
            //COMMON PERMISSIONS (FOR BOTH OF TENANTS AND HOST)

            var pages = context.GetPermissionOrNull(AppPermissions.Pages) ?? context.CreatePermission(AppPermissions.Pages, L("Pages"));
			
			
			

            var mainadmin = pages.CreateChildPermission(AppPermissions.Admin, L("Admin"), multiTenancySides: MultiTenancySides.Tenant);

            var workflows = pages.CreateChildPermission(AppPermissions.Pages_Workflows, L("Workflows"), multiTenancySides: MultiTenancySides.Tenant);
            workflows.CreateChildPermission(AppPermissions.Pages_Workflows_Create, L("CreateNewWorkflow"), multiTenancySides: MultiTenancySides.Tenant);
            workflows.CreateChildPermission(AppPermissions.Pages_Workflows_Edit, L("EditWorkflow"), multiTenancySides: MultiTenancySides.Tenant);
            workflows.CreateChildPermission(AppPermissions.Pages_Workflows_Delete, L("DeleteWorkflow"), multiTenancySides: MultiTenancySides.Tenant);


            var schedules = pages.CreateChildPermission(AppPermissions.Pages_Schedules, L("Schedules"), multiTenancySides: MultiTenancySides.Tenant);
            schedules.CreateChildPermission(AppPermissions.Pages_Schedules_Create, L("CreateNewSchedule"), multiTenancySides: MultiTenancySides.Tenant);
            schedules.CreateChildPermission(AppPermissions.Pages_Schedules_Edit, L("EditSchedule"), multiTenancySides: MultiTenancySides.Tenant);
            schedules.CreateChildPermission(AppPermissions.Pages_Schedules_Delete, L("DeleteSchedule"), multiTenancySides: MultiTenancySides.Tenant);

            var achievementMastersTeamMaps = pages.CreateChildPermission(AppPermissions.Pages_AchievementMastersTeamMaps, L("AchievementMastersTeamMaps"), multiTenancySides: MultiTenancySides.Tenant);
            achievementMastersTeamMaps.CreateChildPermission(AppPermissions.Pages_AchievementMastersTeamMaps_Create, L("CreateNewAchievementMastersTeamMap"), multiTenancySides: MultiTenancySides.Tenant);
            achievementMastersTeamMaps.CreateChildPermission(AppPermissions.Pages_AchievementMastersTeamMaps_Edit, L("EditAchievementMastersTeamMap"), multiTenancySides: MultiTenancySides.Tenant);
            achievementMastersTeamMaps.CreateChildPermission(AppPermissions.Pages_AchievementMastersTeamMaps_Delete, L("DeleteAchievementMastersTeamMap"), multiTenancySides: MultiTenancySides.Tenant);

            var leagueMastersTeamMaps = pages.CreateChildPermission(AppPermissions.Pages_LeagueMastersTeamMaps, L("LeagueMastersTeamMaps"), multiTenancySides: MultiTenancySides.Tenant);
            leagueMastersTeamMaps.CreateChildPermission(AppPermissions.Pages_LeagueMastersTeamMaps_Create, L("CreateNewLeagueMastersTeamMap"), multiTenancySides: MultiTenancySides.Tenant);
            leagueMastersTeamMaps.CreateChildPermission(AppPermissions.Pages_LeagueMastersTeamMaps_Edit, L("EditLeagueMastersTeamMap"), multiTenancySides: MultiTenancySides.Tenant);
            leagueMastersTeamMaps.CreateChildPermission(AppPermissions.Pages_LeagueMastersTeamMaps_Delete, L("DeleteLeagueMastersTeamMap"), multiTenancySides: MultiTenancySides.Tenant);

            var leagueMastersSalesRepMaps = pages.CreateChildPermission(AppPermissions.Pages_LeagueMastersSalesRepMaps, L("LeagueMastersSalesRepMaps"), multiTenancySides: MultiTenancySides.Tenant);
            leagueMastersSalesRepMaps.CreateChildPermission(AppPermissions.Pages_LeagueMastersSalesRepMaps_Create, L("CreateNewLeagueMastersSalesRepMap"), multiTenancySides: MultiTenancySides.Tenant);
            leagueMastersSalesRepMaps.CreateChildPermission(AppPermissions.Pages_LeagueMastersSalesRepMaps_Edit, L("EditLeagueMastersSalesRepMap"), multiTenancySides: MultiTenancySides.Tenant);
            leagueMastersSalesRepMaps.CreateChildPermission(AppPermissions.Pages_LeagueMastersSalesRepMaps_Delete, L("DeleteLeagueMastersSalesRepMap"), multiTenancySides: MultiTenancySides.Tenant);

            var leagueMastersPerformanceMaps = pages.CreateChildPermission(AppPermissions.Pages_LeagueMastersPerformanceMaps, L("LeagueMastersPerformanceMaps"), multiTenancySides: MultiTenancySides.Tenant);
            leagueMastersPerformanceMaps.CreateChildPermission(AppPermissions.Pages_LeagueMastersPerformanceMaps_Create, L("CreateNewLeagueMastersPerformanceMap"), multiTenancySides: MultiTenancySides.Tenant);
            leagueMastersPerformanceMaps.CreateChildPermission(AppPermissions.Pages_LeagueMastersPerformanceMaps_Edit, L("EditLeagueMastersPerformanceMap"), multiTenancySides: MultiTenancySides.Tenant);
            leagueMastersPerformanceMaps.CreateChildPermission(AppPermissions.Pages_LeagueMastersPerformanceMaps_Delete, L("DeleteLeagueMastersPerformanceMap"), multiTenancySides: MultiTenancySides.Tenant);

            var leagueMasters = pages.CreateChildPermission(AppPermissions.Pages_LeagueMasters, L("LeagueMasters"), multiTenancySides: MultiTenancySides.Tenant);
            leagueMasters.CreateChildPermission(AppPermissions.Pages_LeagueMasters_Create, L("CreateNewLeagueMaster"), multiTenancySides: MultiTenancySides.Tenant);
            leagueMasters.CreateChildPermission(AppPermissions.Pages_LeagueMasters_Edit, L("EditLeagueMaster"), multiTenancySides: MultiTenancySides.Tenant);
            leagueMasters.CreateChildPermission(AppPermissions.Pages_LeagueMasters_Delete, L("DeleteLeagueMaster"), multiTenancySides: MultiTenancySides.Tenant);

            var performanceMastersKpiMaps = pages.CreateChildPermission(AppPermissions.Pages_PerformanceMastersKpiMaps, L("PerformanceMastersKpiMaps"), multiTenancySides: MultiTenancySides.Tenant);
            performanceMastersKpiMaps.CreateChildPermission(AppPermissions.Pages_PerformanceMastersKpiMaps_Create, L("CreateNewPerformanceMastersKpiMap"), multiTenancySides: MultiTenancySides.Tenant);
            performanceMastersKpiMaps.CreateChildPermission(AppPermissions.Pages_PerformanceMastersKpiMaps_Edit, L("EditPerformanceMastersKpiMap"), multiTenancySides: MultiTenancySides.Tenant);
            performanceMastersKpiMaps.CreateChildPermission(AppPermissions.Pages_PerformanceMastersKpiMaps_Delete, L("DeletePerformanceMastersKpiMap"), multiTenancySides: MultiTenancySides.Tenant);

            var performanceKpiDetails = pages.CreateChildPermission(AppPermissions.Pages_PerformanceKpiDetails, L("PerformanceKpiDetails"), multiTenancySides: MultiTenancySides.Tenant);
            performanceKpiDetails.CreateChildPermission(AppPermissions.Pages_PerformanceKpiDetails_Create, L("CreateNewPerformanceKpiDetail"), multiTenancySides: MultiTenancySides.Tenant);
            performanceKpiDetails.CreateChildPermission(AppPermissions.Pages_PerformanceKpiDetails_Edit, L("EditPerformanceKpiDetail"), multiTenancySides: MultiTenancySides.Tenant);
            performanceKpiDetails.CreateChildPermission(AppPermissions.Pages_PerformanceKpiDetails_Delete, L("DeletePerformanceKpiDetail"), multiTenancySides: MultiTenancySides.Tenant);

            var performanceKpiHeaders = pages.CreateChildPermission(AppPermissions.Pages_PerformanceKpiHeaders, L("PerformanceKpiHeaders"), multiTenancySides: MultiTenancySides.Tenant);
            performanceKpiHeaders.CreateChildPermission(AppPermissions.Pages_PerformanceKpiHeaders_Create, L("CreateNewPerformanceKpiHeader"), multiTenancySides: MultiTenancySides.Tenant);
            performanceKpiHeaders.CreateChildPermission(AppPermissions.Pages_PerformanceKpiHeaders_Edit, L("EditPerformanceKpiHeader"), multiTenancySides: MultiTenancySides.Tenant);
            performanceKpiHeaders.CreateChildPermission(AppPermissions.Pages_PerformanceKpiHeaders_Delete, L("DeletePerformanceKpiHeader"), multiTenancySides: MultiTenancySides.Tenant);

            var achievementMastersSalesRepMaps = pages.CreateChildPermission(AppPermissions.Pages_AchievementMastersSalesRepMaps, L("AchievementMastersSalesRepMaps"), multiTenancySides: MultiTenancySides.Tenant);
            achievementMastersSalesRepMaps.CreateChildPermission(AppPermissions.Pages_AchievementMastersSalesRepMaps_Create, L("CreateNewAchievementMastersSalesRepMap"), multiTenancySides: MultiTenancySides.Tenant);
            achievementMastersSalesRepMaps.CreateChildPermission(AppPermissions.Pages_AchievementMastersSalesRepMaps_Edit, L("EditAchievementMastersSalesRepMap"), multiTenancySides: MultiTenancySides.Tenant);
            achievementMastersSalesRepMaps.CreateChildPermission(AppPermissions.Pages_AchievementMastersSalesRepMaps_Delete, L("DeleteAchievementMastersSalesRepMap"), multiTenancySides: MultiTenancySides.Tenant);

            var achievementMasters = pages.CreateChildPermission(AppPermissions.Pages_AchievementMasters, L("AchievementMasters"), multiTenancySides: MultiTenancySides.Tenant);
            achievementMasters.CreateChildPermission(AppPermissions.Pages_AchievementMasters_Create, L("CreateNewAchievementMaster"), multiTenancySides: MultiTenancySides.Tenant);
            achievementMasters.CreateChildPermission(AppPermissions.Pages_AchievementMasters_Edit, L("EditAchievementMaster"), multiTenancySides: MultiTenancySides.Tenant);
            achievementMasters.CreateChildPermission(AppPermissions.Pages_AchievementMasters_Delete, L("DeleteAchievementMaster"), multiTenancySides: MultiTenancySides.Tenant);

            var performanceMastersTerritories = pages.CreateChildPermission(AppPermissions.Pages_PerformanceMastersTerritories, L("PerformanceMastersTerritories"), multiTenancySides: MultiTenancySides.Tenant);
            performanceMastersTerritories.CreateChildPermission(AppPermissions.Pages_PerformanceMastersTerritories_Create, L("CreateNewPerformanceMastersTerritorie"), multiTenancySides: MultiTenancySides.Tenant);
            performanceMastersTerritories.CreateChildPermission(AppPermissions.Pages_PerformanceMastersTerritories_Edit, L("EditPerformanceMastersTerritorie"), multiTenancySides: MultiTenancySides.Tenant);
            performanceMastersTerritories.CreateChildPermission(AppPermissions.Pages_PerformanceMastersTerritories_Delete, L("DeletePerformanceMastersTerritorie"), multiTenancySides: MultiTenancySides.Tenant);

            var performanceMastersTeamMaps = pages.CreateChildPermission(AppPermissions.Pages_PerformanceMastersTeamMaps, L("PerformanceMastersTeamMaps"), multiTenancySides: MultiTenancySides.Tenant);
            performanceMastersTeamMaps.CreateChildPermission(AppPermissions.Pages_PerformanceMastersTeamMaps_Create, L("CreateNewPerformanceMastersTeamMap"), multiTenancySides: MultiTenancySides.Tenant);
            performanceMastersTeamMaps.CreateChildPermission(AppPermissions.Pages_PerformanceMastersTeamMaps_Edit, L("EditPerformanceMastersTeamMap"), multiTenancySides: MultiTenancySides.Tenant);
            performanceMastersTeamMaps.CreateChildPermission(AppPermissions.Pages_PerformanceMastersTeamMaps_Delete, L("DeletePerformanceMastersTeamMap"), multiTenancySides: MultiTenancySides.Tenant);

            var performanceMastersSalesRepMaps = pages.CreateChildPermission(AppPermissions.Pages_PerformanceMastersSalesRepMaps, L("PerformanceMastersSalesRepMaps"), multiTenancySides: MultiTenancySides.Tenant);
            performanceMastersSalesRepMaps.CreateChildPermission(AppPermissions.Pages_PerformanceMastersSalesRepMaps_Create, L("CreateNewPerformanceMastersSalesRepMap"), multiTenancySides: MultiTenancySides.Tenant);
            performanceMastersSalesRepMaps.CreateChildPermission(AppPermissions.Pages_PerformanceMastersSalesRepMaps_Edit, L("EditPerformanceMastersSalesRepMap"), multiTenancySides: MultiTenancySides.Tenant);
            performanceMastersSalesRepMaps.CreateChildPermission(AppPermissions.Pages_PerformanceMastersSalesRepMaps_Delete, L("DeletePerformanceMastersSalesRepMap"), multiTenancySides: MultiTenancySides.Tenant);

            var performanceMasters = pages.CreateChildPermission(AppPermissions.Pages_PerformanceMasters, L("PerformanceMasters"), multiTenancySides: MultiTenancySides.Tenant);
            performanceMasters.CreateChildPermission(AppPermissions.Pages_PerformanceMasters_Create, L("CreateNewPerformanceMaster"), multiTenancySides: MultiTenancySides.Tenant);
            performanceMasters.CreateChildPermission(AppPermissions.Pages_PerformanceMasters_Edit, L("EditPerformanceMaster"), multiTenancySides: MultiTenancySides.Tenant);
            performanceMasters.CreateChildPermission(AppPermissions.Pages_PerformanceMasters_Delete, L("DeletePerformanceMaster"), multiTenancySides: MultiTenancySides.Tenant);

            var teamsSalesRepMaps = pages.CreateChildPermission(AppPermissions.Pages_TeamsSalesRepMaps, L("TeamsSalesRepMaps"), multiTenancySides: MultiTenancySides.Tenant);
            teamsSalesRepMaps.CreateChildPermission(AppPermissions.Pages_TeamsSalesRepMaps_Create, L("CreateNewTeamsSalesRepMap"), multiTenancySides: MultiTenancySides.Tenant);
            teamsSalesRepMaps.CreateChildPermission(AppPermissions.Pages_TeamsSalesRepMaps_Edit, L("EditTeamsSalesRepMap"), multiTenancySides: MultiTenancySides.Tenant);
            teamsSalesRepMaps.CreateChildPermission(AppPermissions.Pages_TeamsSalesRepMaps_Delete, L("DeleteTeamsSalesRepMap"), multiTenancySides: MultiTenancySides.Tenant);

            var teamMasters = pages.CreateChildPermission(AppPermissions.Pages_TeamMasters, L("TeamMasters"), multiTenancySides: MultiTenancySides.Tenant);
            teamMasters.CreateChildPermission(AppPermissions.Pages_TeamMasters_Create, L("CreateNewTeamMaster"), multiTenancySides: MultiTenancySides.Tenant);
            teamMasters.CreateChildPermission(AppPermissions.Pages_TeamMasters_Edit, L("EditTeamMaster"), multiTenancySides: MultiTenancySides.Tenant);
            teamMasters.CreateChildPermission(AppPermissions.Pages_TeamMasters_Delete, L("DeleteTeamMaster"), multiTenancySides: MultiTenancySides.Tenant);

            var dealPipelines = pages.CreateChildPermission(AppPermissions.Pages_DealPipelines, L("DealPipelines"), multiTenancySides: MultiTenancySides.Tenant);
            dealPipelines.CreateChildPermission(AppPermissions.Pages_DealPipelines_Create, L("CreateNewDealPipeline"), multiTenancySides: MultiTenancySides.Tenant);
            dealPipelines.CreateChildPermission(AppPermissions.Pages_DealPipelines_Edit, L("EditDealPipeline"), multiTenancySides: MultiTenancySides.Tenant);
            dealPipelines.CreateChildPermission(AppPermissions.Pages_DealPipelines_Delete, L("DeleteDealPipeline"), multiTenancySides: MultiTenancySides.Tenant);

            var deals = pages.CreateChildPermission(AppPermissions.Pages_Deals, L("Deals"), multiTenancySides: MultiTenancySides.Tenant);
            deals.CreateChildPermission(AppPermissions.Pages_Deals_Create, L("CreateNewDeal"), multiTenancySides: MultiTenancySides.Tenant);
            deals.CreateChildPermission(AppPermissions.Pages_Deals_Edit, L("EditDeal"), multiTenancySides: MultiTenancySides.Tenant);
            deals.CreateChildPermission(AppPermissions.Pages_Deals_Delete, L("DeleteDeal"), multiTenancySides: MultiTenancySides.Tenant);

            var dealStages = pages.CreateChildPermission(AppPermissions.Pages_DealStages, L("DealStages"), multiTenancySides: MultiTenancySides.Tenant);
            dealStages.CreateChildPermission(AppPermissions.Pages_DealStages_Create, L("CreateNewDealStage"), multiTenancySides: MultiTenancySides.Tenant);
            dealStages.CreateChildPermission(AppPermissions.Pages_DealStages_Edit, L("EditDealStage"), multiTenancySides: MultiTenancySides.Tenant);
            dealStages.CreateChildPermission(AppPermissions.Pages_DealStages_Delete, L("DeleteDealStage"), multiTenancySides: MultiTenancySides.Tenant);

            var dealTypes = pages.CreateChildPermission(AppPermissions.Pages_DealTypes, L("DealTypes"), multiTenancySides: MultiTenancySides.Host);
            dealTypes.CreateChildPermission(AppPermissions.Pages_DealTypes_Create, L("CreateNewDealType"), multiTenancySides: MultiTenancySides.Host);
            dealTypes.CreateChildPermission(AppPermissions.Pages_DealTypes_Edit, L("EditDealType"), multiTenancySides: MultiTenancySides.Host);
            dealTypes.CreateChildPermission(AppPermissions.Pages_DealTypes_Delete, L("DeleteDealType"), multiTenancySides: MultiTenancySides.Host);

            var promotionsPlaceTerritories = pages.CreateChildPermission(AppPermissions.Pages_PromotionsPlaceTerritories, L("PromotionsPlaceTerritories"));
            promotionsPlaceTerritories.CreateChildPermission(AppPermissions.Pages_PromotionsPlaceTerritories_Create, L("CreateNewPromotionsPlaceTerritories"));
            promotionsPlaceTerritories.CreateChildPermission(AppPermissions.Pages_PromotionsPlaceTerritories_Edit, L("EditPromotionsPlaceTerritories"));
            promotionsPlaceTerritories.CreateChildPermission(AppPermissions.Pages_PromotionsPlaceTerritories_Delete, L("DeletePromotionsPlaceTerritories"));

            var incentiveMasterPlaceClassifications = pages.CreateChildPermission(AppPermissions.Pages_IncentiveMasterPlaceClassifications, L("IncentiveMasterPlaceClassifications"));
            incentiveMasterPlaceClassifications.CreateChildPermission(AppPermissions.Pages_IncentiveMasterPlaceClassifications_Create, L("CreateNewIncentiveMasterPlaceClassification"));
            incentiveMasterPlaceClassifications.CreateChildPermission(AppPermissions.Pages_IncentiveMasterPlaceClassifications_Edit, L("EditIncentiveMasterPlaceClassification"));
            incentiveMasterPlaceClassifications.CreateChildPermission(AppPermissions.Pages_IncentiveMasterPlaceClassifications_Delete, L("DeleteIncentiveMasterPlaceClassification"));

            //var classifications = pages.CreateChildPermission(AppPermissions.Pages_Classifications, L("Classifications"));
            //classifications.CreateChildPermission(AppPermissions.Pages_Classifications_Create, L("CreateNewClassification"));
            //classifications.CreateChildPermission(AppPermissions.Pages_Classifications_Edit, L("EditClassification"));
            //classifications.CreateChildPermission(AppPermissions.Pages_Classifications_Delete, L("DeleteClassification"));

            //var incentiveMasters = pages.CreateChildPermission(AppPermissions.Pages_IncentiveMasters, L("IncentiveMasters"));
            //incentiveMasters.CreateChildPermission(AppPermissions.Pages_IncentiveMasters_Create, L("CreateNewIncentiveMaster"));
            //incentiveMasters.CreateChildPermission(AppPermissions.Pages_IncentiveMasters_Edit, L("EditIncentiveMaster"));
            //incentiveMasters.CreateChildPermission(AppPermissions.Pages_IncentiveMasters_Delete, L("DeleteIncentiveMaster"));

            var promotionsPlaceClassifications = pages.CreateChildPermission(AppPermissions.Pages_PromotionsPlaceClassifications, L("PromotionsPlaceClassifications"));
            promotionsPlaceClassifications.CreateChildPermission(AppPermissions.Pages_PromotionsPlaceClassifications_Create, L("CreateNewPromotionsPlaceClassification"));
            promotionsPlaceClassifications.CreateChildPermission(AppPermissions.Pages_PromotionsPlaceClassifications_Edit, L("EditPromotionsPlaceClassification"));
            promotionsPlaceClassifications.CreateChildPermission(AppPermissions.Pages_PromotionsPlaceClassifications_Delete, L("DeletePromotionsPlaceClassification"));

            var incentiveMasterPlaceTerritories = pages.CreateChildPermission(AppPermissions.Pages_IncentiveMasterPlaceTerritories, L("IncentiveMasterPlaceTerritories"));
            incentiveMasterPlaceTerritories.CreateChildPermission(AppPermissions.Pages_IncentiveMasterPlaceTerritories_Create, L("CreateNewIncentiveMasterPlaceTerritories"));
            incentiveMasterPlaceTerritories.CreateChildPermission(AppPermissions.Pages_IncentiveMasterPlaceTerritories_Edit, L("EditIncentiveMasterPlaceTerritories"));
            incentiveMasterPlaceTerritories.CreateChildPermission(AppPermissions.Pages_IncentiveMasterPlaceTerritories_Delete, L("DeleteIncentiveMasterPlaceTerritories"));

            var offlineSessionIDs = pages.CreateChildPermission(AppPermissions.Pages_OfflineSessionIDs, L("OfflineSessionIDs"));
            offlineSessionIDs.CreateChildPermission(AppPermissions.Pages_OfflineSessionIDs_Create, L("CreateNewOfflineSessionID"));
            offlineSessionIDs.CreateChildPermission(AppPermissions.Pages_OfflineSessionIDs_Edit, L("EditOfflineSessionID"));
            offlineSessionIDs.CreateChildPermission(AppPermissions.Pages_OfflineSessionIDs_Delete, L("DeleteOfflineSessionID"));

            var orderAmountLadgers = pages.CreateChildPermission(AppPermissions.Pages_OrderAmountLadgers, L("OrderAmountLadgers"));
            orderAmountLadgers.CreateChildPermission(AppPermissions.Pages_OrderAmountLadgers_Create, L("CreateNewOrderAmountLadger"));
            orderAmountLadgers.CreateChildPermission(AppPermissions.Pages_OrderAmountLadgers_Edit, L("EditOrderAmountLadger"));
            orderAmountLadgers.CreateChildPermission(AppPermissions.Pages_OrderAmountLadgers_Delete, L("DeleteOrderAmountLadger"));

            var orderPromotionMaps = pages.CreateChildPermission(AppPermissions.Pages_OrderPromotionMaps, L("OrderPromotionMaps"));
            orderPromotionMaps.CreateChildPermission(AppPermissions.Pages_OrderPromotionMaps_Create, L("CreateNewOrderPromotionMap"));
            orderPromotionMaps.CreateChildPermission(AppPermissions.Pages_OrderPromotionMaps_Edit, L("EditOrderPromotionMap"));
            orderPromotionMaps.CreateChildPermission(AppPermissions.Pages_OrderPromotionMaps_Delete, L("DeleteOrderPromotionMap"));

            var promotions = pages.CreateChildPermission(AppPermissions.Pages_Promotions, L("Promotions"));
            promotions.CreateChildPermission(AppPermissions.Pages_Promotions_Create, L("CreateNewPromotion"));
            promotions.CreateChildPermission(AppPermissions.Pages_Promotions_Edit, L("EditPromotion"));
            promotions.CreateChildPermission(AppPermissions.Pages_Promotions_Delete, L("DeletePromotion"));

            var incentiveMasters = pages.CreateChildPermission(AppPermissions.Pages_IncentiveMasters, L("IncentiveMasters"));
            incentiveMasters.CreateChildPermission(AppPermissions.Pages_IncentiveMasters_Create, L("CreateNewIncentiveMaster"));
            incentiveMasters.CreateChildPermission(AppPermissions.Pages_IncentiveMasters_Edit, L("EditIncentiveMaster"));
            incentiveMasters.CreateChildPermission(AppPermissions.Pages_IncentiveMasters_Delete, L("DeleteIncentiveMaster"));

            var emailHandlers = pages.CreateChildPermission(AppPermissions.Pages_EmailHandlers, L("EmailHandlers"));
            emailHandlers.CreateChildPermission(AppPermissions.Pages_EmailHandlers_Create, L("CreateNewEmailHandler"));
            emailHandlers.CreateChildPermission(AppPermissions.Pages_EmailHandlers_Edit, L("EditEmailHandler"));
            emailHandlers.CreateChildPermission(AppPermissions.Pages_EmailHandlers_Delete, L("DeleteEmailHandler"));

            var tanentDemographicInfos = pages.CreateChildPermission(AppPermissions.Pages_TanentDemographicInfos, L("TanentDemographicInfos"));
            tanentDemographicInfos.CreateChildPermission(AppPermissions.Pages_TanentDemographicInfos_Create, L("CreateNewTanentDemographicInfo"));
            tanentDemographicInfos.CreateChildPermission(AppPermissions.Pages_TanentDemographicInfos_Edit, L("EditTanentDemographicInfo"));
            tanentDemographicInfos.CreateChildPermission(AppPermissions.Pages_TanentDemographicInfos_Delete, L("DeleteTanentDemographicInfo"));

            var formResponseImageAnswers = pages.CreateChildPermission(AppPermissions.Pages_FormResponseImageAnswers, L("FormResponseImageAnswers"));
            formResponseImageAnswers.CreateChildPermission(AppPermissions.Pages_FormResponseImageAnswers_Create, L("CreateNewFormResponseImageAnswer"));
            formResponseImageAnswers.CreateChildPermission(AppPermissions.Pages_FormResponseImageAnswers_Edit, L("EditFormResponseImageAnswer"));
            formResponseImageAnswers.CreateChildPermission(AppPermissions.Pages_FormResponseImageAnswers_Delete, L("DeleteFormResponseImageAnswer"));

            var salesRepWorkLogs = pages.CreateChildPermission(AppPermissions.Pages_SalesRepWorkLogs, L("SalesRepWorkLogs"));
            salesRepWorkLogs.CreateChildPermission(AppPermissions.Pages_SalesRepWorkLogs_Create, L("CreateNewSalesRepWorkLog"));
            salesRepWorkLogs.CreateChildPermission(AppPermissions.Pages_SalesRepWorkLogs_Edit, L("EditSalesRepWorkLog"));
            salesRepWorkLogs.CreateChildPermission(AppPermissions.Pages_SalesRepWorkLogs_Delete, L("DeleteSalesRepWorkLog"));

            var salesRepWorkReports = pages.CreateChildPermission(AppPermissions.Pages_SalesRepWorkReports, L("SalesRepWorkReports"));
            salesRepWorkReports.CreateChildPermission(AppPermissions.Pages_SalesRepWorkReports_Create, L("CreateNewSalesRepWorkReport"));
            salesRepWorkReports.CreateChildPermission(AppPermissions.Pages_SalesRepWorkReports_Edit, L("EditSalesRepWorkReport"));
            salesRepWorkReports.CreateChildPermission(AppPermissions.Pages_SalesRepWorkReports_Delete, L("DeleteSalesRepWorkReport"));

            var salesRepresentativeCashLedgers = pages.CreateChildPermission(AppPermissions.Pages_SalesRepresentativeCashLedgers, L("SalesRepresentativeCashLedgers"));
            salesRepresentativeCashLedgers.CreateChildPermission(AppPermissions.Pages_SalesRepresentativeCashLedgers_Create, L("CreateNewSalesRepresentativeCashLedger"));
            salesRepresentativeCashLedgers.CreateChildPermission(AppPermissions.Pages_SalesRepresentativeCashLedgers_Edit, L("EditSalesRepresentativeCashLedger"));
            salesRepresentativeCashLedgers.CreateChildPermission(AppPermissions.Pages_SalesRepresentativeCashLedgers_Delete, L("DeleteSalesRepresentativeCashLedger"));

            var activityComments = pages.CreateChildPermission(AppPermissions.Pages_ActivityComments, L("ActivityComments"));
            activityComments.CreateChildPermission(AppPermissions.Pages_ActivityComments_Create, L("CreateNewActivityComment"));
            activityComments.CreateChildPermission(AppPermissions.Pages_ActivityComments_Edit, L("EditActivityComment"));
            activityComments.CreateChildPermission(AppPermissions.Pages_ActivityComments_Delete, L("DeleteActivityComment"));

            var customerPayments = pages.CreateChildPermission(AppPermissions.Pages_CustomerPayments, L("CustomerPayments"));
            customerPayments.CreateChildPermission(AppPermissions.Pages_CustomerPayments_Create, L("CreateNewCustomerPayment"));
            customerPayments.CreateChildPermission(AppPermissions.Pages_CustomerPayments_Edit, L("EditCustomerPayment"));
            customerPayments.CreateChildPermission(AppPermissions.Pages_CustomerPayments_Delete, L("DeleteCustomerPayment"));

            var paymentCollectionMethods = pages.CreateChildPermission(AppPermissions.Pages_PaymentCollectionMethods, L("PaymentCollectionMethods"));
            paymentCollectionMethods.CreateChildPermission(AppPermissions.Pages_PaymentCollectionMethods_Create, L("CreateNewPaymentCollectionMethod"));
            paymentCollectionMethods.CreateChildPermission(AppPermissions.Pages_PaymentCollectionMethods_Edit, L("EditPaymentCollectionMethod"));
            paymentCollectionMethods.CreateChildPermission(AppPermissions.Pages_PaymentCollectionMethods_Delete, L("DeletePaymentCollectionMethod"));

            var paymentCollections = pages.CreateChildPermission(AppPermissions.Pages_PaymentCollections, L("PaymentCollections"));
            paymentCollections.CreateChildPermission(AppPermissions.Pages_PaymentCollections_Create, L("CreateNewPaymentCollection"));
            paymentCollections.CreateChildPermission(AppPermissions.Pages_PaymentCollections_Edit, L("EditPaymentCollection"));
            paymentCollections.CreateChildPermission(AppPermissions.Pages_PaymentCollections_Delete, L("DeletePaymentCollection"));

            var orderReturns = pages.CreateChildPermission(AppPermissions.Pages_OrderReturns, L("OrderReturns"));
            orderReturns.CreateChildPermission(AppPermissions.Pages_OrderReturns_Create, L("CreateNewOrderReturn"));
            orderReturns.CreateChildPermission(AppPermissions.Pages_OrderReturns_Edit, L("EditOrderReturn"));
            orderReturns.CreateChildPermission(AppPermissions.Pages_OrderReturns_Delete, L("DeleteOrderReturn"));

            var teamMemberUserRoles = pages.CreateChildPermission(AppPermissions.Pages_TeamMemberUserRoles, L("TeamMemberUserRoles"));
            teamMemberUserRoles.CreateChildPermission(AppPermissions.Pages_TeamMemberUserRoles_Create, L("CreateNewTeamMemberUserRole"));
            teamMemberUserRoles.CreateChildPermission(AppPermissions.Pages_TeamMemberUserRoles_Edit, L("EditTeamMemberUserRole"));
            teamMemberUserRoles.CreateChildPermission(AppPermissions.Pages_TeamMemberUserRoles_Delete, L("DeleteTeamMemberUserRole"));

            var teamMemberTerritories = pages.CreateChildPermission(AppPermissions.Pages_TeamMemberTerritories, L("TeamMemberTerritories"));
            teamMemberTerritories.CreateChildPermission(AppPermissions.Pages_TeamMemberTerritories_Create, L("CreateNewTeamMemberTerritorie"));
            teamMemberTerritories.CreateChildPermission(AppPermissions.Pages_TeamMemberTerritories_Edit, L("EditTeamMemberTerritorie"));
            teamMemberTerritories.CreateChildPermission(AppPermissions.Pages_TeamMemberTerritories_Delete, L("DeleteTeamMemberTerritorie"));

            var teamMembers = pages.CreateChildPermission(AppPermissions.Pages_TeamMembers, L("TeamMembers"));
            teamMembers.CreateChildPermission(AppPermissions.Pages_TeamMembers_Create, L("CreateNewTeamMember"));
            teamMembers.CreateChildPermission(AppPermissions.Pages_TeamMembers_Edit, L("EditTeamMember"));
            teamMembers.CreateChildPermission(AppPermissions.Pages_TeamMembers_Delete, L("DeleteTeamMember"));

            var virRoles = pages.CreateChildPermission(AppPermissions.Pages_VirRoles, L("VirRoles"));
            virRoles.CreateChildPermission(AppPermissions.Pages_VirRoles_Create, L("CreateNewVirRole"));
            virRoles.CreateChildPermission(AppPermissions.Pages_VirRoles_Edit, L("EditVirRole"));
            virRoles.CreateChildPermission(AppPermissions.Pages_VirRoles_Delete, L("DeleteVirRole"));

            var stockNotifications = pages.CreateChildPermission(AppPermissions.Pages_StockNotifications, L("StockNotifications"));
            stockNotifications.CreateChildPermission(AppPermissions.Pages_StockNotifications_Create, L("CreateNewStockNotification"));
            stockNotifications.CreateChildPermission(AppPermissions.Pages_StockNotifications_Edit, L("EditStockNotification"));
            stockNotifications.CreateChildPermission(AppPermissions.Pages_StockNotifications_Delete, L("DeleteStockNotification"));

            var territoryUserMaps = pages.CreateChildPermission(AppPermissions.Pages_TerritoryUserMaps, L("TerritoryUserMaps"));
            territoryUserMaps.CreateChildPermission(AppPermissions.Pages_TerritoryUserMaps_Create, L("CreateNewTerritoryUserMap"));
            territoryUserMaps.CreateChildPermission(AppPermissions.Pages_TerritoryUserMaps_Edit, L("EditTerritoryUserMap"));
            territoryUserMaps.CreateChildPermission(AppPermissions.Pages_TerritoryUserMaps_Delete, L("DeleteTerritoryUserMap"));

            var formResponseProductAnswers = pages.CreateChildPermission(AppPermissions.Pages_FormResponseProductAnswers, L("FormResponseProductAnswers"));
            formResponseProductAnswers.CreateChildPermission(AppPermissions.Pages_FormResponseProductAnswers_Create, L("CreateNewFormResponseProductAnswer"));
            formResponseProductAnswers.CreateChildPermission(AppPermissions.Pages_FormResponseProductAnswers_Edit, L("EditFormResponseProductAnswer"));
            formResponseProductAnswers.CreateChildPermission(AppPermissions.Pages_FormResponseProductAnswers_Delete, L("DeleteFormResponseProductAnswer"));

            var formResponseOptionAnswers = pages.CreateChildPermission(AppPermissions.Pages_FormResponseOptionAnswers, L("FormResponseOptionAnswers"));
            formResponseOptionAnswers.CreateChildPermission(AppPermissions.Pages_FormResponseOptionAnswers_Create, L("CreateNewFormResponseOptionAnswer"));
            formResponseOptionAnswers.CreateChildPermission(AppPermissions.Pages_FormResponseOptionAnswers_Edit, L("EditFormResponseOptionAnswer"));
            formResponseOptionAnswers.CreateChildPermission(AppPermissions.Pages_FormResponseOptionAnswers_Delete, L("DeleteFormResponseOptionAnswer"));

            var formResponseAnswers = pages.CreateChildPermission(AppPermissions.Pages_FormResponseAnswers, L("FormResponseAnswers"));
            formResponseAnswers.CreateChildPermission(AppPermissions.Pages_FormResponseAnswers_Create, L("CreateNewFormResponseAnswer"));
            formResponseAnswers.CreateChildPermission(AppPermissions.Pages_FormResponseAnswers_Edit, L("EditFormResponseAnswer"));
            formResponseAnswers.CreateChildPermission(AppPermissions.Pages_FormResponseAnswers_Delete, L("DeleteFormResponseAnswer"));

            var formResponses = pages.CreateChildPermission(AppPermissions.Pages_FormResponses, L("FormResponses"));
            formResponses.CreateChildPermission(AppPermissions.Pages_FormResponses_Create, L("CreateNewFormResponse"));
            formResponses.CreateChildPermission(AppPermissions.Pages_FormResponses_Edit, L("EditFormResponse"));
            formResponses.CreateChildPermission(AppPermissions.Pages_FormResponses_Delete, L("DeleteFormResponse"));

            var tenantInfos = pages.CreateChildPermission(AppPermissions.Pages_TenantInfos, L("TenantInfos"));
            tenantInfos.CreateChildPermission(AppPermissions.Pages_TenantInfos_Create, L("CreateNewTenantInfo"));
            tenantInfos.CreateChildPermission(AppPermissions.Pages_TenantInfos_Edit, L("EditTenantInfo"));
            tenantInfos.CreateChildPermission(AppPermissions.Pages_TenantInfos_Delete, L("DeleteTenantInfo"));

            var rolePermissionVirs = pages.CreateChildPermission(AppPermissions.Pages_RolePermissionVirs, L("RolePermissionVirs"));
            rolePermissionVirs.CreateChildPermission(AppPermissions.Pages_RolePermissionVirs_Create, L("CreateNewRolePermissionVir"));
            rolePermissionVirs.CreateChildPermission(AppPermissions.Pages_RolePermissionVirs_Edit, L("EditRolePermissionVir"));
            rolePermissionVirs.CreateChildPermission(AppPermissions.Pages_RolePermissionVirs_Delete, L("DeleteRolePermissionVir"));

            var formEntitySalesRepMaps = pages.CreateChildPermission(AppPermissions.Pages_FormEntitySalesRepMaps, L("FormEntitySalesRepMaps"));
            formEntitySalesRepMaps.CreateChildPermission(AppPermissions.Pages_FormEntitySalesRepMaps_Create, L("CreateNewFormEntitySalesRepMap"));
            formEntitySalesRepMaps.CreateChildPermission(AppPermissions.Pages_FormEntitySalesRepMaps_Edit, L("EditFormEntitySalesRepMap"));
            formEntitySalesRepMaps.CreateChildPermission(AppPermissions.Pages_FormEntitySalesRepMaps_Delete, L("DeleteFormEntitySalesRepMap"));

            var formEntityTeritoryMaps = pages.CreateChildPermission(AppPermissions.Pages_FormEntityTeritoryMaps, L("FormEntityTeritoryMaps"));
            formEntityTeritoryMaps.CreateChildPermission(AppPermissions.Pages_FormEntityTeritoryMaps_Create, L("CreateNewFormEntityTeritoryMap"));
            formEntityTeritoryMaps.CreateChildPermission(AppPermissions.Pages_FormEntityTeritoryMaps_Edit, L("EditFormEntityTeritoryMap"));
            formEntityTeritoryMaps.CreateChildPermission(AppPermissions.Pages_FormEntityTeritoryMaps_Delete, L("DeleteFormEntityTeritoryMap"));

            var expenseImages = pages.CreateChildPermission(AppPermissions.Pages_ExpenseImages, L("ExpenseImages"));
            expenseImages.CreateChildPermission(AppPermissions.Pages_ExpenseImages_Create, L("CreateNewExpenseImage"));
            expenseImages.CreateChildPermission(AppPermissions.Pages_ExpenseImages_Edit, L("EditExpenseImage"));
            expenseImages.CreateChildPermission(AppPermissions.Pages_ExpenseImages_Delete, L("DeleteExpenseImage"));

            var vanMembers = pages.CreateChildPermission(AppPermissions.Pages_VanMembers, L("VanMembers"));
            vanMembers.CreateChildPermission(AppPermissions.Pages_VanMembers_Create, L("CreateNewVanMember"));
            vanMembers.CreateChildPermission(AppPermissions.Pages_VanMembers_Edit, L("EditVanMember"));
            vanMembers.CreateChildPermission(AppPermissions.Pages_VanMembers_Delete, L("DeleteVanMember"));

            var placeRepresentatives = pages.CreateChildPermission(AppPermissions.Pages_PlaceRepresentatives, L("PlaceRepresentatives"));
            placeRepresentatives.CreateChildPermission(AppPermissions.Pages_PlaceRepresentatives_Create, L("CreateNewPlaceRepresentative"));
            placeRepresentatives.CreateChildPermission(AppPermissions.Pages_PlaceRepresentatives_Edit, L("EditPlaceRepresentative"));
            placeRepresentatives.CreateChildPermission(AppPermissions.Pages_PlaceRepresentatives_Delete, L("DeletePlaceRepresentative"));

            var customerSequenceIds = pages.CreateChildPermission(AppPermissions.Pages_CustomerSequenceIds, L("CustomerSequenceIds"));
            customerSequenceIds.CreateChildPermission(AppPermissions.Pages_CustomerSequenceIds_Create, L("CreateNewCustomerSequenceId"));
            customerSequenceIds.CreateChildPermission(AppPermissions.Pages_CustomerSequenceIds_Edit, L("EditCustomerSequenceId"));
            customerSequenceIds.CreateChildPermission(AppPermissions.Pages_CustomerSequenceIds_Delete, L("DeleteCustomerSequenceId"));

            var backOfficeUserTerritories = pages.CreateChildPermission(AppPermissions.Pages_BackOfficeUserTerritories, L("BackOfficeUserTerritories"));
            backOfficeUserTerritories.CreateChildPermission(AppPermissions.Pages_BackOfficeUserTerritories_Create, L("CreateNewBackOfficeUserTerritorie"));
            backOfficeUserTerritories.CreateChildPermission(AppPermissions.Pages_BackOfficeUserTerritories_Edit, L("EditBackOfficeUserTerritorie"));
            backOfficeUserTerritories.CreateChildPermission(AppPermissions.Pages_BackOfficeUserTerritories_Delete, L("DeleteBackOfficeUserTerritorie"));

            var createBackOfficeUsers = pages.CreateChildPermission(AppPermissions.Pages_CreateBackOfficeUsers, L("CreateBackOfficeUsers"));
            createBackOfficeUsers.CreateChildPermission(AppPermissions.Pages_CreateBackOfficeUsers_Create, L("CreateNewCreateBackOfficeUser"));
            createBackOfficeUsers.CreateChildPermission(AppPermissions.Pages_CreateBackOfficeUsers_Edit, L("EditCreateBackOfficeUser"));
            createBackOfficeUsers.CreateChildPermission(AppPermissions.Pages_CreateBackOfficeUsers_Delete, L("DeleteCreateBackOfficeUser"));

            var backOfficeUserRolePermissions = pages.CreateChildPermission(AppPermissions.Pages_BackOfficeUserRolePermissions, L("BackOfficeUserRolePermissions"));
            backOfficeUserRolePermissions.CreateChildPermission(AppPermissions.Pages_BackOfficeUserRolePermissions_Create, L("CreateNewBackOfficeUserRolePermission"));
            backOfficeUserRolePermissions.CreateChildPermission(AppPermissions.Pages_BackOfficeUserRolePermissions_Edit, L("EditBackOfficeUserRolePermission"));
            backOfficeUserRolePermissions.CreateChildPermission(AppPermissions.Pages_BackOfficeUserRolePermissions_Delete, L("DeleteBackOfficeUserRolePermission"));

            var backOfficeUserRoles = pages.CreateChildPermission(AppPermissions.Pages_BackOfficeUserRoles, L("BackOfficeUserRoles"));
            backOfficeUserRoles.CreateChildPermission(AppPermissions.Pages_BackOfficeUserRoles_Create, L("CreateNewBackOfficeUserRole"));
            backOfficeUserRoles.CreateChildPermission(AppPermissions.Pages_BackOfficeUserRoles_Edit, L("EditBackOfficeUserRole"));
            backOfficeUserRoles.CreateChildPermission(AppPermissions.Pages_BackOfficeUserRoles_Delete, L("DeleteBackOfficeUserRole"));

            var orderDetailwithUoMQuanities = pages.CreateChildPermission(AppPermissions.Pages_OrderDetailwithUoMQuanities, L("OrderDetailwithUoMQuanities"));
            orderDetailwithUoMQuanities.CreateChildPermission(AppPermissions.Pages_OrderDetailwithUoMQuanities_Create, L("CreateNewOrderDetailwithUoMQuanity"));
            orderDetailwithUoMQuanities.CreateChildPermission(AppPermissions.Pages_OrderDetailwithUoMQuanities_Edit, L("EditOrderDetailwithUoMQuanity"));
            orderDetailwithUoMQuanities.CreateChildPermission(AppPermissions.Pages_OrderDetailwithUoMQuanities_Delete, L("DeleteOrderDetailwithUoMQuanity"));

            var orderProductDetails = pages.CreateChildPermission(AppPermissions.Pages_OrderProductDetails, L("OrderProductDetails"));
            orderProductDetails.CreateChildPermission(AppPermissions.Pages_OrderProductDetails_Create, L("CreateNewOrderProductDetail"));
            orderProductDetails.CreateChildPermission(AppPermissions.Pages_OrderProductDetails_Edit, L("EditOrderProductDetail"));
            orderProductDetails.CreateChildPermission(AppPermissions.Pages_OrderProductDetails_Delete, L("DeleteOrderProductDetail"));

            var signatureFiles = pages.CreateChildPermission(AppPermissions.Pages_SignatureFiles, L("SignatureFiles"));
            signatureFiles.CreateChildPermission(AppPermissions.Pages_SignatureFiles_Create, L("CreateNewSignatureFile"));
            signatureFiles.CreateChildPermission(AppPermissions.Pages_SignatureFiles_Edit, L("EditSignatureFile"));
            signatureFiles.CreateChildPermission(AppPermissions.Pages_SignatureFiles_Delete, L("DeleteSignatureFile"));

            var orderDetails = pages.CreateChildPermission(AppPermissions.Pages_OrderDetails, L("OrderDetails"));
            orderDetails.CreateChildPermission(AppPermissions.Pages_OrderDetails_Create, L("CreateNewOrderDetail"));
            orderDetails.CreateChildPermission(AppPermissions.Pages_OrderDetails_Edit, L("EditOrderDetail"));
            orderDetails.CreateChildPermission(AppPermissions.Pages_OrderDetails_Delete, L("DeleteOrderDetail"));

            var activityAttachmentDetails = pages.CreateChildPermission(AppPermissions.Pages_ActivityAttachmentDetails, L("ActivityAttachmentDetails"));
            activityAttachmentDetails.CreateChildPermission(AppPermissions.Pages_ActivityAttachmentDetails_Create, L("CreateNewActivityAttachmentDetail"));
            activityAttachmentDetails.CreateChildPermission(AppPermissions.Pages_ActivityAttachmentDetails_Edit, L("EditActivityAttachmentDetail"));
            activityAttachmentDetails.CreateChildPermission(AppPermissions.Pages_ActivityAttachmentDetails_Delete, L("DeleteActivityAttachmentDetail"));

            var activities = pages.CreateChildPermission(AppPermissions.Pages_Activities, L("Activities"));
            activities.CreateChildPermission(AppPermissions.Pages_Activities_Create, L("CreateNewActivity"));
            activities.CreateChildPermission(AppPermissions.Pages_Activities_Edit, L("EditActivity"));
            activities.CreateChildPermission(AppPermissions.Pages_Activities_Delete, L("DeleteActivity"));

            var stochTransactionDetailSummaries = pages.CreateChildPermission(AppPermissions.Pages_StochTransactionDetailSummaries, L("StochTransactionDetailSummaries"));
            stochTransactionDetailSummaries.CreateChildPermission(AppPermissions.Pages_StochTransactionDetailSummaries_Create, L("CreateNewStochTransactionDetailSummary"));
            stochTransactionDetailSummaries.CreateChildPermission(AppPermissions.Pages_StochTransactionDetailSummaries_Edit, L("EditStochTransactionDetailSummary"));
            stochTransactionDetailSummaries.CreateChildPermission(AppPermissions.Pages_StochTransactionDetailSummaries_Delete, L("DeleteStochTransactionDetailSummary"));

            var stockTransactionWithUomquantities = pages.CreateChildPermission(AppPermissions.Pages_StockTransactionWithUomquantities, L("StockTransactionWithUomquantities"));
            stockTransactionWithUomquantities.CreateChildPermission(AppPermissions.Pages_StockTransactionWithUomquantities_Create, L("CreateNewStockTransactionWithUomquantity"));
            stockTransactionWithUomquantities.CreateChildPermission(AppPermissions.Pages_StockTransactionWithUomquantities_Edit, L("EditStockTransactionWithUomquantity"));
            stockTransactionWithUomquantities.CreateChildPermission(AppPermissions.Pages_StockTransactionWithUomquantities_Delete, L("DeleteStockTransactionWithUomquantity"));

            var stockTransactionProductDetails = pages.CreateChildPermission(AppPermissions.Pages_StockTransactionProductDetails, L("StockTransactionProductDetails"));
            stockTransactionProductDetails.CreateChildPermission(AppPermissions.Pages_StockTransactionProductDetails_Create, L("CreateNewStockTransactionProductDetail"));
            stockTransactionProductDetails.CreateChildPermission(AppPermissions.Pages_StockTransactionProductDetails_Edit, L("EditStockTransactionProductDetail"));
            stockTransactionProductDetails.CreateChildPermission(AppPermissions.Pages_StockTransactionProductDetails_Delete, L("DeleteStockTransactionProductDetail"));

            var stockTransactionSummaries = pages.CreateChildPermission(AppPermissions.Pages_StockTransactionSummaries, L("StockTransactionSummaries"));
            stockTransactionSummaries.CreateChildPermission(AppPermissions.Pages_StockTransactionSummaries_Create, L("CreateNewStockTransactionSummary"));
            stockTransactionSummaries.CreateChildPermission(AppPermissions.Pages_StockTransactionSummaries_Edit, L("EditStockTransactionSummary"));
            stockTransactionSummaries.CreateChildPermission(AppPermissions.Pages_StockTransactionSummaries_Delete, L("DeleteStockTransactionSummary"));

            var placeIDAndSalesRepMappings = pages.CreateChildPermission(AppPermissions.Pages_PlaceIDAndSalesRepMappings, L("PlaceIDAndSalesRepMappings"));
            placeIDAndSalesRepMappings.CreateChildPermission(AppPermissions.Pages_PlaceIDAndSalesRepMappings_Create, L("CreateNewPlaceIDAndSalesRepMapping"));
            placeIDAndSalesRepMappings.CreateChildPermission(AppPermissions.Pages_PlaceIDAndSalesRepMappings_Edit, L("EditPlaceIDAndSalesRepMapping"));
            placeIDAndSalesRepMappings.CreateChildPermission(AppPermissions.Pages_PlaceIDAndSalesRepMappings_Delete, L("DeletePlaceIDAndSalesRepMapping"));

            var placeContactDetails = pages.CreateChildPermission(AppPermissions.Pages_PlaceContactDetails, L("PlaceContactDetails"));
            placeContactDetails.CreateChildPermission(AppPermissions.Pages_PlaceContactDetails_Create, L("CreateNewPlaceContactDetail"));
            placeContactDetails.CreateChildPermission(AppPermissions.Pages_PlaceContactDetails_Edit, L("EditPlaceContactDetail"));
            placeContactDetails.CreateChildPermission(AppPermissions.Pages_PlaceContactDetails_Delete, L("DeletePlaceContactDetail"));

            var places = pages.CreateChildPermission(AppPermissions.Pages_Places, L("Places"));
            places.CreateChildPermission(AppPermissions.Pages_Places_Create, L("CreateNewPlace"));
            places.CreateChildPermission(AppPermissions.Pages_Places_Edit, L("EditPlace"));
            places.CreateChildPermission(AppPermissions.Pages_Places_Delete, L("DeletePlace"));

            var expenses = pages.CreateChildPermission(AppPermissions.Pages_Expenses, L("Expenses"));
            expenses.CreateChildPermission(AppPermissions.Pages_Expenses_Create, L("CreateNewExpense"));
            expenses.CreateChildPermission(AppPermissions.Pages_Expenses_Edit, L("EditExpense"));
            expenses.CreateChildPermission(AppPermissions.Pages_Expenses_Delete, L("DeleteExpense"));
            var productImages = pages.CreateChildPermission(AppPermissions.Pages_ProductImages, L("ProductImages"));
            productImages.CreateChildPermission(AppPermissions.Pages_ProductImages_Create, L("CreateNewProductImage"));
            productImages.CreateChildPermission(AppPermissions.Pages_ProductImages_Edit, L("EditProductImage"));
            productImages.CreateChildPermission(AppPermissions.Pages_ProductImages_Delete, L("DeleteProductImage"));

            var productAttributeMappings = pages.CreateChildPermission(AppPermissions.Pages_ProductAttributeMappings, L("ProductAttributeMappings"));
            productAttributeMappings.CreateChildPermission(AppPermissions.Pages_ProductAttributeMappings_Create, L("CreateNewProductAttributeMapping"));
            productAttributeMappings.CreateChildPermission(AppPermissions.Pages_ProductAttributeMappings_Edit, L("EditProductAttributeMapping"));
            productAttributeMappings.CreateChildPermission(AppPermissions.Pages_ProductAttributeMappings_Delete, L("DeleteProductAttributeMapping"));

            var productAttributeValues = pages.CreateChildPermission(AppPermissions.Pages_ProductAttributeValues, L("ProductAttributeValues"));
            productAttributeValues.CreateChildPermission(AppPermissions.Pages_ProductAttributeValues_Create, L("CreateNewProductAttributeValue"));
            productAttributeValues.CreateChildPermission(AppPermissions.Pages_ProductAttributeValues_Edit, L("EditProductAttributeValue"));
            productAttributeValues.CreateChildPermission(AppPermissions.Pages_ProductAttributeValues_Delete, L("DeleteProductAttributeValue"));

            var productAttributes = pages.CreateChildPermission(AppPermissions.Pages_ProductAttributes, L("ProductAttributes"));
            productAttributes.CreateChildPermission(AppPermissions.Pages_ProductAttributes_Create, L("CreateNewProductAttribute"));
            productAttributes.CreateChildPermission(AppPermissions.Pages_ProductAttributes_Edit, L("EditProductAttribute"));
            productAttributes.CreateChildPermission(AppPermissions.Pages_ProductAttributes_Delete, L("DeleteProductAttribute"));

            var products = pages.CreateChildPermission(AppPermissions.Pages_Products, L("Products"));
            products.CreateChildPermission(AppPermissions.Pages_Products_Create, L("CreateNewProduct"));
            products.CreateChildPermission(AppPermissions.Pages_Products_Edit, L("EditProduct"));
            products.CreateChildPermission(AppPermissions.Pages_Products_Delete, L("DeleteProduct"));

            var productParents = pages.CreateChildPermission(AppPermissions.Pages_ProductParents, L("ProductParents"));
            productParents.CreateChildPermission(AppPermissions.Pages_ProductParents_Create, L("CreateNewProductParent"));
            productParents.CreateChildPermission(AppPermissions.Pages_ProductParents_Edit, L("EditProductParent"));
            productParents.CreateChildPermission(AppPermissions.Pages_ProductParents_Delete, L("DeleteProductParent"));

            var productCategories = pages.CreateChildPermission(AppPermissions.Pages_ProductCategories, L("ProductCategories"));
            productCategories.CreateChildPermission(AppPermissions.Pages_ProductCategories_Create, L("CreateNewProductCategory"));
            productCategories.CreateChildPermission(AppPermissions.Pages_ProductCategories_Edit, L("EditProductCategory"));
            productCategories.CreateChildPermission(AppPermissions.Pages_ProductCategories_Delete, L("DeleteProductCategory"));

            var incentives = pages.CreateChildPermission(AppPermissions.Pages_Incentives, L("Incentives"));
            incentives.CreateChildPermission(AppPermissions.Pages_Incentives_Create, L("CreateNewIncentive"));
            incentives.CreateChildPermission(AppPermissions.Pages_Incentives_Edit, L("EditIncentive"));
            incentives.CreateChildPermission(AppPermissions.Pages_Incentives_Delete, L("DeleteIncentive"));

            var vanUserMaps = pages.CreateChildPermission(AppPermissions.Pages_VanUserMaps, L("VanUserMaps"));
            vanUserMaps.CreateChildPermission(AppPermissions.Pages_VanUserMaps_Create, L("CreateNewVanUserMap"));
            vanUserMaps.CreateChildPermission(AppPermissions.Pages_VanUserMaps_Edit, L("EditVanUserMap"));
            vanUserMaps.CreateChildPermission(AppPermissions.Pages_VanUserMaps_Delete, L("DeleteVanUserMap"));

            var vans = pages.CreateChildPermission(AppPermissions.Pages_Vans, L("Vans"));
            vans.CreateChildPermission(AppPermissions.Pages_Vans_Create, L("CreateNewVan"));
            vans.CreateChildPermission(AppPermissions.Pages_Vans_Edit, L("EditVan"));
            vans.CreateChildPermission(AppPermissions.Pages_Vans_Delete, L("DeleteVan"));

            var companyInfos = pages.CreateChildPermission(AppPermissions.Pages_CompanyInfos, L("CompanyInfos"));
            companyInfos.CreateChildPermission(AppPermissions.Pages_CompanyInfos_Create, L("CreateNewCompanyInfo"));
            companyInfos.CreateChildPermission(AppPermissions.Pages_CompanyInfos_Edit, L("EditCompanyInfo"));
            companyInfos.CreateChildPermission(AppPermissions.Pages_CompanyInfos_Delete, L("DeleteCompanyInfo"));

            var distanceUnits = pages.CreateChildPermission(AppPermissions.Pages_DistanceUnits, L("DistanceUnits"));
            distanceUnits.CreateChildPermission(AppPermissions.Pages_DistanceUnits_Create, L("CreateNewDistanceUnit"));
            distanceUnits.CreateChildPermission(AppPermissions.Pages_DistanceUnits_Edit, L("EditDistanceUnit"));
            distanceUnits.CreateChildPermission(AppPermissions.Pages_DistanceUnits_Delete, L("DeleteDistanceUnit"));

            var uomMasters = pages.CreateChildPermission(AppPermissions.Pages_UomMasters, L("UomMasters"));
            uomMasters.CreateChildPermission(AppPermissions.Pages_UomMasters_Create, L("CreateNewUomMaster"));
            uomMasters.CreateChildPermission(AppPermissions.Pages_UomMasters_Edit, L("EditUomMaster"));
            uomMasters.CreateChildPermission(AppPermissions.Pages_UomMasters_Delete, L("DeleteUomMaster"));

            var classifications = pages.CreateChildPermission(AppPermissions.Pages_Classifications, L("Classifications"));
            classifications.CreateChildPermission(AppPermissions.Pages_Classifications_Create, L("CreateNewClassification"));
            classifications.CreateChildPermission(AppPermissions.Pages_Classifications_Edit, L("EditClassification"));
            classifications.CreateChildPermission(AppPermissions.Pages_Classifications_Delete, L("DeleteClassification"));

            var territories = pages.CreateChildPermission(AppPermissions.Pages_Territories, L("Territories"));
            territories.CreateChildPermission(AppPermissions.Pages_Territories_Create, L("CreateNewTerritorie"));
            territories.CreateChildPermission(AppPermissions.Pages_Territories_Edit, L("EditTerritorie"));
            territories.CreateChildPermission(AppPermissions.Pages_Territories_Delete, L("DeleteTerritorie"));

            var formQuestionValues = pages.CreateChildPermission(AppPermissions.Pages_FormQuestionValues, L("FormQuestionValues"));
            formQuestionValues.CreateChildPermission(AppPermissions.Pages_FormQuestionValues_Create, L("CreateNewFormQuestionValues"));
            formQuestionValues.CreateChildPermission(AppPermissions.Pages_FormQuestionValues_Edit, L("EditFormQuestionValues"));
            formQuestionValues.CreateChildPermission(AppPermissions.Pages_FormQuestionValues_Delete, L("DeleteFormQuestionValues"));

            var formQuestions = pages.CreateChildPermission(AppPermissions.Pages_FormQuestions, L("FormQuestions"));
            formQuestions.CreateChildPermission(AppPermissions.Pages_FormQuestions_Create, L("CreateNewFormQuestion"));
            formQuestions.CreateChildPermission(AppPermissions.Pages_FormQuestions_Edit, L("EditFormQuestion"));
            formQuestions.CreateChildPermission(AppPermissions.Pages_FormQuestions_Delete, L("DeleteFormQuestion"));

            var questionTypes = pages.CreateChildPermission(AppPermissions.Pages_QuestionTypes, L("QuestionTypes"));
            questionTypes.CreateChildPermission(AppPermissions.Pages_QuestionTypes_Create, L("CreateNewQuestionType"));
            questionTypes.CreateChildPermission(AppPermissions.Pages_QuestionTypes_Edit, L("EditQuestionType"));
            questionTypes.CreateChildPermission(AppPermissions.Pages_QuestionTypes_Delete, L("DeleteQuestionType"));

            var formMasters = pages.CreateChildPermission(AppPermissions.Pages_FormMasters, L("FormMasters"));
            formMasters.CreateChildPermission(AppPermissions.Pages_FormMasters_Create, L("CreateNewFormMaster"));
            formMasters.CreateChildPermission(AppPermissions.Pages_FormMasters_Edit, L("EditFormMaster"));
            formMasters.CreateChildPermission(AppPermissions.Pages_FormMasters_Delete, L("DeleteFormMaster"));

            var categoryMasters = pages.CreateChildPermission(AppPermissions.Pages_CategoryMasters, L("CategoryMasters"));
            categoryMasters.CreateChildPermission(AppPermissions.Pages_CategoryMasters_Create, L("CreateNewCategoryMaster"));
            categoryMasters.CreateChildPermission(AppPermissions.Pages_CategoryMasters_Edit, L("EditCategoryMaster"));
            categoryMasters.CreateChildPermission(AppPermissions.Pages_CategoryMasters_Delete, L("DeleteCategoryMaster"));
			
			
            //StockTransactionBackOffice PERMISSIONS

            var stockTransactions =
            pages.CreateChildPermission(AppPermissions.AcceptReplinishStock, L("AcceptReplinishStock"));
            pages.CreateChildPermission(AppPermissions.CreateStockTransactionOut, L("CreateStockTransactionOut"));
            pages.CreateChildPermission(AppPermissions.AcceptStockTransferOut, L("AcceptStockTransferOut"));
            pages.CreateChildPermission(AppPermissions.CreateStockCount, L("CreateStockCount"));
            pages.CreateChildPermission(AppPermissions.AcceptStockCount, L("AcceptStockCount"));
            pages.CreateChildPermission(AppPermissions.CreateStockRequest, L("CreateStockRequest"));
            pages.CreateChildPermission(AppPermissions.AcceptStockRequest, L("AcceptStockRequest"));
            pages.CreateChildPermission(AppPermissions.CreateStockUpdateRequest, L("CreateStockUpdateRequest"));

            //CustomePermissions

            pages.CreateChildPermission(AppPermissions.SalesInvoice, L("SalesInvoice"));
            pages.CreateChildPermission(AppPermissions.Settlement, L("Settlement"));

            pages.CreateChildPermission(AppPermissions.Schedule, L("Schedule"));
            pages.CreateChildPermission(AppPermissions.Summary, L("Summary"));

            pages.CreateChildPermission(AppPermissions.Reports, L("Reports"));

            //var MyActivities = pages.CreateChildPermission(AppPermissions.MyActivities, L("MyActivities"));
            //var MyPlaces = pages.CreateChildPermission(AppPermissions.MyPlaces, L("MyPlaces"));
            //var Products = pages.CreateChildPermission(AppPermissions.Products, L("Products"));
            //var Schedule = pages.CreateChildPermission(AppPermissions.Schedule, L("Schedule"));

            //var TimeMileageHistory = pages.CreateChildPermission(AppPermissions.TimeMileageHistory, L("TimeMileageHistory"));
            //var ProductInformation = pages.CreateChildPermission(AppPermissions.ProductInformation, L("ProductInformation"));
            //var Summary = pages.CreateChildPermission(AppPermissions.Summary, L("Summary"));
            //var VehiclesVans = pages.CreateChildPermission(AppPermissions.VehiclesVans, L("VehiclesVans"));

            //var Reimbursements = pages.CreateChildPermission(AppPermissions.Reimbursements, L("Reimbursements"));
            //var Incentives = pages.CreateChildPermission(AppPermissions.Incentives, L("Incentives"));
            //var TeamMembers = pages.CreateChildPermission(AppPermissions.TeamMembers, L("TeamMembers"));

            //var Promotions = pages.CreateChildPermission(AppPermissions.Promotions, L("Promotions"));
            //var Reports = pages.CreateChildPermission(AppPermissions.Reports, L("Reports"));
            //var SalesInvoice = pages.CreateChildPermission(AppPermissions.SalesInvoice, L("SalesInvoice"));

            //var PaymentCollection = pages.CreateChildPermission(AppPermissions.PaymentCollection, L("PaymentCollection"));
            //var Settlement = pages.CreateChildPermission(AppPermissions.Settlement, L("Settlement"));
            //var Forms = pages.CreateChildPermission(AppPermissions.Forms, L("Forms"));

            //var StockTransactions = pages.CreateChildPermission(AppPermissions.StockTransactions, L("StockTransactions"));
            //var CanApplyDiscount = pages.CreateChildPermission(AppPermissions.CanApplyDiscount, L("CanApplyDiscount"));


////Default ASp.net
			
            pages.CreateChildPermission(AppPermissions.Pages_DemoUiComponents, L("DemoUiComponents"));

            var administration = pages.CreateChildPermission(AppPermissions.Pages_Administration, L("Administration"));

            var roles = administration.CreateChildPermission(AppPermissions.Pages_Administration_Roles, L("Roles"));
            roles.CreateChildPermission(AppPermissions.Pages_Administration_Roles_Create, L("CreatingNewRole"));
            roles.CreateChildPermission(AppPermissions.Pages_Administration_Roles_Edit, L("EditingRole"));
            roles.CreateChildPermission(AppPermissions.Pages_Administration_Roles_Delete, L("DeletingRole"));

            var users = administration.CreateChildPermission(AppPermissions.Pages_Administration_Users, L("Users"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_Create, L("CreatingNewUser"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_Edit, L("EditingUser"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_Delete, L("DeletingUser"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_ChangePermissions, L("ChangingPermissions"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_Impersonation, L("LoginForUsers"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_Unlock, L("Unlock"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_ChangeProfilePicture, L("UpdateUsersProfilePicture"));

            var languages = administration.CreateChildPermission(AppPermissions.Pages_Administration_Languages, L("Languages"));
            languages.CreateChildPermission(AppPermissions.Pages_Administration_Languages_Create, L("CreatingNewLanguage"), multiTenancySides: _isMultiTenancyEnabled ? MultiTenancySides.Host : MultiTenancySides.Tenant);
            languages.CreateChildPermission(AppPermissions.Pages_Administration_Languages_Edit, L("EditingLanguage"), multiTenancySides: _isMultiTenancyEnabled ? MultiTenancySides.Host : MultiTenancySides.Tenant);
            languages.CreateChildPermission(AppPermissions.Pages_Administration_Languages_Delete, L("DeletingLanguages"), multiTenancySides: _isMultiTenancyEnabled ? MultiTenancySides.Host : MultiTenancySides.Tenant);
            languages.CreateChildPermission(AppPermissions.Pages_Administration_Languages_ChangeTexts, L("ChangingTexts"));
            languages.CreateChildPermission(AppPermissions.Pages_Administration_Languages_ChangeDefaultLanguage, L("ChangeDefaultLanguage"));
            
            administration.CreateChildPermission(AppPermissions.Pages_Administration_AuditLogs, L("AuditLogs"));

            var organizationUnits = administration.CreateChildPermission(AppPermissions.Pages_Administration_OrganizationUnits, L("OrganizationUnits"));
            organizationUnits.CreateChildPermission(AppPermissions.Pages_Administration_OrganizationUnits_ManageOrganizationTree, L("ManagingOrganizationTree"));
            organizationUnits.CreateChildPermission(AppPermissions.Pages_Administration_OrganizationUnits_ManageMembers, L("ManagingMembers"));
            organizationUnits.CreateChildPermission(AppPermissions.Pages_Administration_OrganizationUnits_ManageRoles, L("ManagingRoles"));

            administration.CreateChildPermission(AppPermissions.Pages_Administration_UiCustomization, L("VisualSettings"));

            var webhooks = administration.CreateChildPermission(AppPermissions.Pages_Administration_WebhookSubscription, L("Webhooks"));
            webhooks.CreateChildPermission(AppPermissions.Pages_Administration_WebhookSubscription_Create, L("CreatingWebhooks"));
            webhooks.CreateChildPermission(AppPermissions.Pages_Administration_WebhookSubscription_Edit, L("EditingWebhooks"));
            webhooks.CreateChildPermission(AppPermissions.Pages_Administration_WebhookSubscription_ChangeActivity, L("ChangingWebhookActivity"));
            webhooks.CreateChildPermission(AppPermissions.Pages_Administration_WebhookSubscription_Detail, L("DetailingSubscription"));
            webhooks.CreateChildPermission(AppPermissions.Pages_Administration_Webhook_ListSendAttempts, L("ListingSendAttempts"));
            webhooks.CreateChildPermission(AppPermissions.Pages_Administration_Webhook_ResendWebhook, L("ResendingWebhook"));

            var dynamicProperties = administration.CreateChildPermission(AppPermissions.Pages_Administration_DynamicProperties, L("DynamicProperties"));
            dynamicProperties.CreateChildPermission(AppPermissions.Pages_Administration_DynamicProperties_Create, L("CreatingDynamicProperties"));
            dynamicProperties.CreateChildPermission(AppPermissions.Pages_Administration_DynamicProperties_Edit, L("EditingDynamicProperties"));
            dynamicProperties.CreateChildPermission(AppPermissions.Pages_Administration_DynamicProperties_Delete, L("DeletingDynamicProperties"));

            var dynamicPropertyValues = dynamicProperties.CreateChildPermission(AppPermissions.Pages_Administration_DynamicPropertyValue, L("DynamicPropertyValue"));
            dynamicPropertyValues.CreateChildPermission(AppPermissions.Pages_Administration_DynamicPropertyValue_Create, L("CreatingDynamicPropertyValue"));
            dynamicPropertyValues.CreateChildPermission(AppPermissions.Pages_Administration_DynamicPropertyValue_Edit, L("EditingDynamicPropertyValue"));
            dynamicPropertyValues.CreateChildPermission(AppPermissions.Pages_Administration_DynamicPropertyValue_Delete, L("DeletingDynamicPropertyValue"));

            var dynamicEntityProperties = dynamicProperties.CreateChildPermission(AppPermissions.Pages_Administration_DynamicEntityProperties, L("DynamicEntityProperties"));
            dynamicEntityProperties.CreateChildPermission(AppPermissions.Pages_Administration_DynamicEntityProperties_Create, L("CreatingDynamicEntityProperties"));
            dynamicEntityProperties.CreateChildPermission(AppPermissions.Pages_Administration_DynamicEntityProperties_Edit, L("EditingDynamicEntityProperties"));
            dynamicEntityProperties.CreateChildPermission(AppPermissions.Pages_Administration_DynamicEntityProperties_Delete, L("DeletingDynamicEntityProperties"));

            var dynamicEntityPropertyValues = dynamicProperties.CreateChildPermission(AppPermissions.Pages_Administration_DynamicEntityPropertyValue, L("EntityDynamicPropertyValue"));
            dynamicEntityPropertyValues.CreateChildPermission(AppPermissions.Pages_Administration_DynamicEntityPropertyValue_Create, L("CreatingDynamicEntityPropertyValue"));
            dynamicEntityPropertyValues.CreateChildPermission(AppPermissions.Pages_Administration_DynamicEntityPropertyValue_Edit, L("EditingDynamicEntityPropertyValue"));
            dynamicEntityPropertyValues.CreateChildPermission(AppPermissions.Pages_Administration_DynamicEntityPropertyValue_Delete, L("DeletingDynamicEntityPropertyValue"));

            var massNotification = administration.CreateChildPermission(AppPermissions.Pages_Administration_MassNotification, L("MassNotifications"));
            massNotification.CreateChildPermission(AppPermissions.Pages_Administration_MassNotification_Create, L("MassNotificationCreate"));
            
            //TENANT-SPECIFIC PERMISSIONS

            pages.CreateChildPermission(AppPermissions.Pages_Tenant_Dashboard, L("Dashboard"), multiTenancySides: MultiTenancySides.Tenant);

            administration.CreateChildPermission(AppPermissions.Pages_Administration_Tenant_Settings, L("Settings"), multiTenancySides: MultiTenancySides.Tenant);
            administration.CreateChildPermission(AppPermissions.Pages_Administration_Tenant_SubscriptionManagement, L("Subscription"), multiTenancySides: MultiTenancySides.Tenant);

            //HOST-SPECIFIC PERMISSIONS

            var editions = pages.CreateChildPermission(AppPermissions.Pages_Editions, L("Editions"), multiTenancySides: MultiTenancySides.Host);
            editions.CreateChildPermission(AppPermissions.Pages_Editions_Create, L("CreatingNewEdition"), multiTenancySides: MultiTenancySides.Host);
            editions.CreateChildPermission(AppPermissions.Pages_Editions_Edit, L("EditingEdition"), multiTenancySides: MultiTenancySides.Host);
            editions.CreateChildPermission(AppPermissions.Pages_Editions_Delete, L("DeletingEdition"), multiTenancySides: MultiTenancySides.Host);
            editions.CreateChildPermission(AppPermissions.Pages_Editions_MoveTenantsToAnotherEdition, L("MoveTenantsToAnotherEdition"), multiTenancySides: MultiTenancySides.Host);

            var tenants = pages.CreateChildPermission(AppPermissions.Pages_Tenants, L("Tenants"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(AppPermissions.Pages_Tenants_Create, L("CreatingNewTenant"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(AppPermissions.Pages_Tenants_Edit, L("EditingTenant"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(AppPermissions.Pages_Tenants_ChangeFeatures, L("ChangingFeatures"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(AppPermissions.Pages_Tenants_Delete, L("DeletingTenant"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(AppPermissions.Pages_Tenants_Impersonation, L("LoginForTenants"), multiTenancySides: MultiTenancySides.Host);

            administration.CreateChildPermission(AppPermissions.Pages_Administration_Host_Settings, L("Settings"), multiTenancySides: MultiTenancySides.Host);
            
            var maintenance = administration.CreateChildPermission(AppPermissions.Pages_Administration_Host_Maintenance, L("Maintenance"), multiTenancySides: _isMultiTenancyEnabled ? MultiTenancySides.Host : MultiTenancySides.Tenant);
            maintenance.CreateChildPermission(AppPermissions.Pages_Administration_NewVersion_Create, L("SendNewVersionNotification"));
            
            administration.CreateChildPermission(AppPermissions.Pages_Administration_HangfireDashboard, L("HangfireDashboard"), multiTenancySides: _isMultiTenancyEnabled ? MultiTenancySides.Host : MultiTenancySides.Tenant);
            administration.CreateChildPermission(AppPermissions.Pages_Administration_Host_Dashboard, L("Dashboard"), multiTenancySides: MultiTenancySides.Host);
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, ArchConsts.LocalizationSourceName);
        }
    }
}
