using BepInEx;
using BepInEx.Logging;
using BepInEx.Configuration;
using UnityEngine;
using DiskCardGame;
using HarmonyLib;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace LifeCost.lib
{
    public static class DialogueHelper
    {
        public static DialogueEvent.LineSet CreateLineSet(
            string[] lineString,
            Emotion emotion = Emotion.Neutral,
            TextDisplayer.LetterAnimation animation = TextDisplayer.LetterAnimation.None,
            P03AnimationController.Face p03Face = P03AnimationController.Face.Default,
            int speakerIndex = 0)
        {
            return new DialogueEvent.LineSet
            {
                lines = (from s in lineString
                         select new DialogueEvent.Line
                         {
                             text = s,
                             emotion = emotion,
                             letterAnimation = animation,
                             p03Face = p03Face,
                             speakerIndex = speakerIndex
                         }).ToList<DialogueEvent.Line>()
            };
        }

        // Token: 0x0600006B RID: 107 RVA: 0x00002F74 File Offset: 0x00001174
        public static void AddOrModifySimpleDialogEvent(string eventId, string line, TextDisplayer.LetterAnimation? animation = null, Emotion? emotion = null)
        {
            string[] lines = new string[]
            {
                line
            };
            DialogueHelper.AddOrModifySimpleDialogEvent(eventId, lines, null, animation, emotion, "NewRunDealtDeckDefault");
        }

        // Token: 0x0600006C RID: 108 RVA: 0x00002FA0 File Offset: 0x000011A0
        private static void SyncLineCollection(List<DialogueEvent.Line> curLines, string[] newLines, TextDisplayer.LetterAnimation? animation, Emotion? emotion)
        {
            while (curLines.Count > newLines.Length)
            {
                curLines.RemoveAt(curLines.Count - 1);
            }
            for (int i = 0; i < curLines.Count; i++)
            {
                curLines[i].text = newLines[i];
            }
            for (int j = curLines.Count; j < newLines.Length; j++)
            {
                DialogueEvent.Line line = DialogueHelper.CloneLine(curLines[0]);
                line.text = newLines[j];
                bool flag = animation != null;
                if (flag)
                {
                    line.letterAnimation = animation.Value;
                }
                bool flag2 = emotion != null;
                if (flag2)
                {
                    line.emotion = emotion.Value;
                }
                curLines.Add(line);
            }
        }

        // Token: 0x0600006D RID: 109 RVA: 0x00003064 File Offset: 0x00001264
        public static void AddOrModifySimpleDialogEvent(string eventId, string[] lines, string[][] repeatLines = null, TextDisplayer.LetterAnimation? animation = null, Emotion? emotion = null, string template = "NewRunDealtDeckDefault")
        {
            bool flag = false;
            DialogueEvent dialogueEvent = DialogueDataUtil.Data.GetEvent(eventId);
            bool flag2 = dialogueEvent == null;
            if (flag2)
            {
                flag = true;
                dialogueEvent = DialogueHelper.CloneDialogueEvent(DialogueDataUtil.Data.GetEvent(template), eventId, false);
                while (dialogueEvent.mainLines.lines.Count > lines.Length)
                {
                    dialogueEvent.mainLines.lines.RemoveAt(lines.Length);
                }
            }
            DialogueHelper.SyncLineCollection(dialogueEvent.mainLines.lines, lines, animation, emotion);
            bool flag3 = repeatLines == null;
            if (flag3)
            {
                dialogueEvent.repeatLines.Clear();
            }
            else
            {
                while (dialogueEvent.repeatLines.Count > repeatLines.Length)
                {
                    dialogueEvent.repeatLines.RemoveAt(dialogueEvent.repeatLines.Count - 1);
                }
                for (int i = 0; i < dialogueEvent.repeatLines.Count; i++)
                {
                    DialogueHelper.SyncLineCollection(dialogueEvent.repeatLines[i].lines, repeatLines[i], animation, emotion);
                }
            }
            bool flag4 = flag;
            if (flag4)
            {
                DialogueDataUtil.Data.events.Add(dialogueEvent);
            }
        }

        // Token: 0x0600006E RID: 110 RVA: 0x0000318C File Offset: 0x0000138C
        public static DialogueEvent.Line CloneLine(DialogueEvent.Line line)
        {
            return new DialogueEvent.Line
            {
                p03Face = line.p03Face,
                emotion = line.emotion,
                letterAnimation = line.letterAnimation,
                speakerIndex = line.speakerIndex,
                text = line.text,
                specialInstruction = line.specialInstruction,
                storyCondition = line.storyCondition,
                storyConditionMustBeMet = line.storyConditionMustBeMet
            };
        }

        // Token: 0x0600006F RID: 111 RVA: 0x00003204 File Offset: 0x00001404
        public static DialogueEvent CloneDialogueEvent(DialogueEvent dialogueEvent, string newId, bool includeRepeat = false)
        {
            DialogueEvent dialogueEvent2 = new DialogueEvent
            {
                id = newId,
                groupId = dialogueEvent.groupId,
                mainLines = new DialogueEvent.LineSet(),
                speakers = new List<DialogueEvent.Speaker>(),
                repeatLines = new List<DialogueEvent.LineSet>()
            };
            foreach (DialogueEvent.Line line in dialogueEvent.mainLines.lines)
            {
                dialogueEvent2.mainLines.lines.Add(DialogueHelper.CloneLine(line));
            }
            if (includeRepeat)
            {
                foreach (DialogueEvent.LineSet lineSet in dialogueEvent.repeatLines)
                {
                    DialogueEvent.LineSet lineSet2 = new DialogueEvent.LineSet();
                    foreach (DialogueEvent.Line line2 in lineSet.lines)
                    {
                        lineSet2.lines.Add(DialogueHelper.CloneLine(line2));
                    }
                    dialogueEvent2.repeatLines.Add(lineSet2);
                }
            }
            foreach (DialogueEvent.Speaker item in dialogueEvent.speakers)
            {
                dialogueEvent2.speakers.Add(item);
            }
            return dialogueEvent2;
        }
    }
}
