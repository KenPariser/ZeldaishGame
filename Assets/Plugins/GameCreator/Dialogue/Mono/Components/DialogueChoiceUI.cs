namespace GameCreator.Dialogue
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;

    public class DialogueChoiceUI : MonoBehaviour
    {
        public Text text;
        public Button button;
        public Graphic color;

        // INITIALIZERS: --------------------------------------------------------------------------

        public void Setup(DatabaseDialogue.ConfigData config, DialogueItemChoiceGroup item,
            int i, bool disabled)
        {
            if (this.text != null) this.text.text = item.children[i].GetContent();
            if (this.button != null) this.button.interactable = !disabled;

            this.AssignButtonChoice(item, i);

            if (item.children[i].IsRevisit() && this.color != null && config != null)
            {
                this.color.color = new Color(
                    this.color.color.r,
                    this.color.color.g,
                    this.color.color.b,
                    this.color.color.a * config.revisitChoiceOpacity
                );
            }
        }

        // PRIVATE METHODS: -----------------------------------------------------------------------

        protected void AssignButtonChoice(DialogueItemChoiceGroup item, int index)
        {
            if (this.button == null) return;
            this.button.onClick.AddListener(() => item.OnMakeChoice(index));
        }
    }
}