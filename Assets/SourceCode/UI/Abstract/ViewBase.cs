using FairyGUI;

namespace UI.Abstract
{
    public abstract class ViewBase
    {
        protected GComponent Panel;
        protected abstract string PackageName { get; }
        protected abstract string ComponentName { get; }
        protected abstract void OnCreateUI();
        protected virtual void OnShow() { }
        protected virtual void OnHide() { }

        public void CreateUI()
        {
            UIPackage.AddPackage(PackageName);
            Panel = UIPackage.CreateObject(PackageName, ComponentName).asCom;
            GRoot.inst.AddChild(Panel);
            Panel.SetSize(GRoot.inst.width, GRoot.inst.height);
            Panel.AddRelation(GRoot.inst, RelationType.Size);
            OnCreateUI();
            Hide();
        }

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
    }
}