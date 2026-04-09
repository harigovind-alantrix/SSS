using Core.Models;

namespace UI.Abstract
{
    public interface IPresenter
    {
        GameState TargetState { get; }
        void Initialize();
        void Show();
        void Hide();
    }
}