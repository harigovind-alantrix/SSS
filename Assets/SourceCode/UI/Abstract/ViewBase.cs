using FairyGUI;

namespace UI.Abstract
{
    public abstract class ViewBase
    {
        protected GComponent Panel;

        public abstract void CreateUI();

        public void Show()
        {
            if (Panel == null) return;
            Panel.visible = true;
            OnShow();
        }

        public void Hide()
        {
            if (Panel == null) return;

            Panel.visible = false;
            OnHide();
        }

        protected virtual void OnShow() { }
        protected virtual void OnHide() { }

        protected GComponent CreatePanel(string packageName, string componentName)
        {
            UIPackage.AddPackage(packageName);
            var panel = UIPackage.CreateObject(packageName, componentName).asCom;

            GRoot.inst.AddChild(panel);
            panel.SetSize(GRoot.inst.width, GRoot.inst.height);
            panel.AddRelation(GRoot.inst, RelationType.Size);

            return panel;
        }
    }
}