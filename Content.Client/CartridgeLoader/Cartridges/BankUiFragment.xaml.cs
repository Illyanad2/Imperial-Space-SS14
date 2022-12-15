using Robust.Client.AutoGenerated;
using Robust.Client.UserInterface.Controls;
using Robust.Client.UserInterface.XAML;
using Content.Shared.CartridgeLoader.Cartridges;

namespace Content.Client.CartridgeLoader.Cartridges;

[GenerateTypedNameReferences]
public sealed partial class BankUiFragment : BoxContainer
{
    public BankUiFragment()
    {
        RobustXamlLoader.Load(this);
    }
    public void UpdateState(BankUiState state)
    {
        bool noAccount = true;
        if (state.LinkedAccountNumber != null)
            noAccount = false;

        NoLinkedAccount.Visible = noAccount;
        LinkedAccount.Visible = !noAccount;
        LinkedAccountNumberLabel.Text = Loc.GetString("bank-program-account-number", ("number", state.LinkedAccountNumber ?? "???"));
        LinkedAccountBalanceLabel.Text = Loc.GetString("bank-program-account-balance", ("balance", state.LinkedAccountBalance ?? 0), ("currencySymbol", state.CurrencySymbol ?? ""));
    }
}