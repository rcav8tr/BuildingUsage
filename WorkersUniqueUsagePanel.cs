﻿using UnityEngine;
using System;
using System.Collections.Generic;

namespace BuildingUsage
{
    /// <summary>
    /// a panel to display unique buildings worker usage
    /// </summary>
    public class WorkersUniqueUsagePanel : UsagePanel
    {
        /// <summary>
        /// Start is called once after the panel is created
        /// set up and populate the panel with UI components
        /// </summary>
        public override void Start()
        {
            // do base processing
            base.Start();

            try
            {
                // set the panel name
                name = GetType().ToString();

                // create a button to return to main panel
                CreateReturnFromDetailButton("Return to Workers");

                // define list of usage types for this panel that are on building prefabs
                List<UsageType> usageTypes = new List<UsageType>();
                int buildingPrefabCount = PrefabCollection<BuildingInfo>.LoadedCount();
                for (uint index = 0; index < buildingPrefabCount; index++)
                {
                    // get the prefab
                    BuildingInfo prefab = PrefabCollection<BuildingInfo>.GetLoaded(index);

                    // get the usage type for the prefab
                    UsageType usageType = GetWorkersUniqueUsageType(prefab);

                    // if not None and not already in the list, add it to the list
                    if (usageType != UsageType.None && !usageTypes.Contains(usageType))
                    {
                        usageTypes.Add(usageType);
                    }
                }

                // create the usage groups
                // at least one of Basic and Level are in the base game, so there is no logic on those headings
                CreateGroupHeading("Basic Unique");
                CreateUsageGroupIfDefined(UsageType.WorkersUniqueLandmark,          usageTypes);
                CreateUsageGroupIfDefined(UsageType.WorkersUniqueTourismLeisure,    usageTypes);
                CreateUsageGroupIfDefined(UsageType.WorkersUniqueWinterUnique,      usageTypes);
                CreateUsageGroupIfDefined(UsageType.WorkersUniqueFootball,          usageTypes);
                CreateUsageGroupIfDefined(UsageType.WorkersUniqueConcert,           usageTypes);

                CreateGroupHeading("Level Unique");
                CreateUsageGroupIfDefined(UsageType.WorkersUniqueLevel1,            usageTypes);
                CreateUsageGroupIfDefined(UsageType.WorkersUniqueLevel2,            usageTypes);
                CreateUsageGroupIfDefined(UsageType.WorkersUniqueLevel3,            usageTypes);
                CreateUsageGroupIfDefined(UsageType.WorkersUniqueLevel4,            usageTypes);
                CreateUsageGroupIfDefined(UsageType.WorkersUniqueLevel5,            usageTypes);
                CreateUsageGroupIfDefined(UsageType.WorkersUniqueLevel6,            usageTypes);

                if (usageTypes.Contains(UsageType.WorkersUniqueContentCreator) || usageTypes.Contains(UsageType.WorkersUniqueCastle)) CreateGroupHeading("Other Unique");
                CreateUsageGroupIfDefined(UsageType.WorkersUniqueContentCreator,    usageTypes);
                CreateUsageGroupIfDefined(UsageType.WorkersUniqueCastle,            usageTypes);

                // associate each building AI type with its usage type(s) and usage count routine(s)
                // associate building AIs even if corresponding DLC is not installed (there will simply be no buildings with that AI)
                AssociateBuildingAI<MonumentAI       >(UsageType.UseLogic1, GetUsageCountWorkersService<MonumentAI       >);
                AssociateBuildingAI<AnimalMonumentAI >(UsageType.UseLogic1, GetUsageCountWorkersService<AnimalMonumentAI >);
                AssociateBuildingAI<PrivateAirportAI >(UsageType.UseLogic1, GetUsageCountWorkersService<PrivateAirportAI >);
                AssociateBuildingAI<ChirpwickCastleAI>(UsageType.UseLogic1, GetUsageCountWorkersService<ChirpwickCastleAI>);
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
        }

        /// <summary>
        /// get the usage type for a building when logic 1 is required
        /// </summary>
        protected override UsageType GetUsageType1ForBuilding(ushort buildingID, ref Building data)
        {
            // logic depends on building AI type
            Type buildingAIType = data.Info.m_buildingAI.GetType();
            if (buildingAIType == typeof(MonumentAI         ) ||
                buildingAIType == typeof(AnimalMonumentAI   ) ||
                buildingAIType == typeof(PrivateAirportAI   ) ||
                buildingAIType == typeof(ChirpwickCastleAI  ))
            {
                return GetWorkersUniqueUsageType(data.Info);
            }

            Debug.LogError($"Unhandled building AI type [{buildingAIType.ToString()}] when getting usage type with logic.");
            return UsageType.None;
        }

        /// <summary>
        /// get the usage type for a building when logic 2 is required
        /// </summary>
        protected override UsageType GetUsageType2ForBuilding(ushort buildingID, ref Building data)
        {
            // usage type not determined with above logic
            Type buildingAIType = data.Info.m_buildingAI.GetType();
            Debug.LogError($"Unhandled building AI type [{buildingAIType.ToString()}] when getting usage type with logic.");
            return UsageType.None;
        }

        /// <summary>
        /// get the usage type for a vehicle when logic is required
        /// </summary>
        protected override UsageType GetUsageTypeForVehicle(ushort vehicleID, ref Vehicle data)
        {
            Type vehicleAIType = data.Info.m_vehicleAI.GetType();
            Debug.LogError($"Unhandled vehicle AI type [{vehicleAIType.ToString()}] when getting usage type with logic.");
            return UsageType.None;
        }

    }
}
