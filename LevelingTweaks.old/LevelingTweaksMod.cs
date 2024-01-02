using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.GameComponents;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

namespace LevelingTweaks
{
    public class LevelingTweaksMod : MBSubModuleBase
    {
        protected override void OnGameStart(Game game, IGameStarter gameStarterObject)
        {
            //if (!(game.GameType is Campaign))
            //    return;
            //this.ReplaceModel<DefaultCharacterDevelopmentModel, RoarsCharacterDevelopmentModel>(gameStarterObject);
        }

        //protected void ReplaceModel<TBaseType, TChildType>(IGameStarter gameStarterObject)
        //  where TBaseType : GameModel
        //  where TChildType : TBaseType
        //{
        //    if (!(gameStarterObject.Models is IList<GameModel> models))
        //        return;
        //    bool flag = false;
        //    for (int index = 0; index < models.Count; ++index)
        //    {
        //        if (models[index] is TBaseType)
        //        {
        //            flag = true;
        //            if (!(models[index] is TChildType))
        //                models[index] = (GameModel)Activator.CreateInstance<TChildType>();
        //        }
        //    }
        //    if (flag)
        //        return;
        //    gameStarterObject.AddModel((GameModel)Activator.CreateInstance<TChildType>());
        //}
    }
}

