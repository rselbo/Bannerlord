using MCM.Abstractions.Base.Global;
using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.GameComponents;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

namespace BoostMod
{
    internal class BoostModModule : MBSubModuleBase
    {
        public static BoostSettings Settings { get; private set; }

        protected override void OnBeforeInitialModuleScreenSetAsRoot()
        {
            Settings = GlobalSettings<BoostSettings>.Instance;
        }

        protected override void OnGameStart(Game game, IGameStarter gameStarterObject)
        {
            base.OnGameStart(game, gameStarterObject);
            if (!(game.GameType is Campaign))
                return;
            this.AddModels(gameStarterObject);
            gameStarterObject.AddModel(new BoostMovementSpeed());
            gameStarterObject.AddModel(new BoostSiegeEventModel());
        }

        protected virtual void AddModels(IGameStarter gameStarterObject) => this.ReplaceModel<DefaultBuildingConstructionModel, BoostModModel>(gameStarterObject);

        protected void ReplaceModel<TBaseType, TChildType>(IGameStarter gameStarterObject)
          where TBaseType : GameModel
          where TChildType : TBaseType
        {
            if (!(gameStarterObject.Models is IList<GameModel> models))
                return;
            bool flag = false;
            for (int index = 0; index < models.Count; ++index)
            {
                if (models[index] is TBaseType)
                {
                    flag = true;
                    if (!(models[index] is TChildType))
                        models[index] = (GameModel)Activator.CreateInstance<TChildType>();
                }
            }
            if (flag)
                return;
            gameStarterObject.AddModel((GameModel)Activator.CreateInstance<TChildType>());
        }
    }
}
