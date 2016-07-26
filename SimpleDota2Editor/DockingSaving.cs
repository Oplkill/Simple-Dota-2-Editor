using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using KV_reloaded;
using SimpleDota2Editor.Panels;
using WeifenLuo.WinFormsUI.Docking;

namespace SimpleDota2Editor
{
    public static class DockingSaving
    {
        public static string PanelSettingsFileName = "PanelSettings.kv";
        public static string OpenObjectsPanelFileName = "sd2e_LastOpenObjects.kv";

        public static void SaveMainPanelsDocking()
        {
            KVToken token = new KVToken
            {
                Type = KVTokenType.KVblock,
                Key = "PanelSettings",
                Children = new List<KVToken>()
            };

            token.Children.Add(new KVToken() { Type = KVTokenType.KVsimple, Key = "HeroesPanel", Value = ((int)AllPanels.DockHeroesView).ToString() });
            token.Children.Add(new KVToken() { Type = KVTokenType.KVsimple, Key = "UnitsPanel", Value = ((int)AllPanels.DockUnitsView).ToString() });
            token.Children.Add(new KVToken() { Type = KVTokenType.KVsimple, Key = "ItemsPanel", Value = ((int)AllPanels.DockItemsView).ToString() });
            token.Children.Add(new KVToken() { Type = KVTokenType.KVsimple, Key = "AbilityPanel", Value = ((int)AllPanels.DockAbilityView).ToString() });
            token.Children.Add(new KVToken() { Type = KVTokenType.KVsimple, Key = "AbilityOverPanel", Value = ((int)AllPanels.DockAbilityOverrideView).ToString() });
            token.ForceSetStandartStyle();

            var file = new StreamWriter(PanelSettingsFileName, false);
            file.WriteLine(token.ToString());
            file.Close();
        }

        public static void LoadMainPanelsDocking()
        {
            if (!File.Exists(PanelSettingsFileName)) return;

            try
            {
                var file = new StreamReader(PanelSettingsFileName);
                string text = file.ReadToEnd();
                file.Close();

                var tokens = TokenAnalizer.AnaliseText(text).First();

                AllPanels.DockHeroesView = (DockState)int.Parse(tokens.GetChild("HeroesPanel").Value);
                AllPanels.DockUnitsView = (DockState)int.Parse(tokens.GetChild("UnitsPanel").Value);
                AllPanels.DockItemsView = (DockState)int.Parse(tokens.GetChild("ItemsPanel").Value);
                AllPanels.DockAbilityView = (DockState)int.Parse(tokens.GetChild("AbilityPanel").Value);
                AllPanels.DockAbilityOverrideView = (DockState)int.Parse(tokens.GetChild("AbilityOverPanel").Value);
            }
            catch (Exception e)
            {
                //todo вставить сюда логирование

                return;
            }
        }

        //---------------

        public static void SaveOpenObjects(string projectPath)
        {
            KVToken token = new KVToken
            {
                Type = KVTokenType.KVblock,
                Key = "LastOpenObjects",
                Children = new List<KVToken>()
            };

            var panels = AllPanels.PrimaryDocking.Contents.Where(doc =>
                    doc.DockHandler.Form is IEditor);

            foreach (var doc in panels)
            {
                var form = (IEditor) doc.DockHandler.Form;

                KVToken tok = new KVToken
                {
                    Type = KVTokenType.KVblock,
                    Key = form.PanelName,
                    Children = new List<KVToken>()
                };

                tok.Children.Add(new KVToken() { Type = KVTokenType.KVsimple, Key = "ObjectTypeId", Value = ((int)form.ObjectType).ToString() });
                tok.Children.Add(new KVToken() { Type = KVTokenType.KVsimple, Key = "EditorId", Value = ((int)form.EditorType).ToString() });
                tok.Children.Add(new KVToken() { Type = KVTokenType.KVsimple, Key = "DockingId", Value = ((int)doc.DockHandler.DockState).ToString() });

                token.Children.Add(tok);
            }

            token.ForceSetStandartStyle();

            var file = new StreamWriter(projectPath + OpenObjectsPanelFileName, false);
            file.WriteLine(token.ToString());
            file.Close();
        }
    }
}