using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.SandBox.GameComponents.Map;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

namespace RefiningMod
{
    public class RefiningMod : MBSubModuleBase
    {
        protected override void OnGameStart(Game game, IGameStarter gameStarterObject)
        {
            if (!(game.GameType is Campaign))
                return;
            this.ReplaceModel<DefaultSmithingModel, RoarsSmithingModelModel>(gameStarterObject);
        }

        protected void ReplaceModel<TBaseType, TChildType>(IGameStarter gameStarterObject)
          where TBaseType : GameModel
          where TChildType : TBaseType
        {
            if (!(gameStarterObject.Models is IList<GameModel> models))
                return;
            bool flag = false;
            for (int index = 0; index < ((ICollection<GameModel>)models).Count; ++index)
            {
                if (models[index] is TBaseType)
                {
                    flag = true;
                    if (!(models[index] is TChildType))
                        models[index] = (GameModel)(object)Activator.CreateInstance<TChildType>();
                }
            }
            if (flag)
                return;
            gameStarterObject.AddModel((GameModel)(object)Activator.CreateInstance<TChildType>());
        }
    }
}