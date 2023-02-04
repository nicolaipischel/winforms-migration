using CodeSpire.Api.Client;
using coIT.BewirbDich.Winforms.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Berechnungsart = coIT.BewirbDich.Winforms.Domain.Berechnungsart;
using Dokument = coIT.BewirbDich.Winforms.Domain.Dokument;
using Dokumenttyp = coIT.BewirbDich.Winforms.Domain.Dokumenttyp;
using Risiko = coIT.BewirbDich.Winforms.Domain.Risiko;

namespace coIT.BewirbDich.Winforms.UI;

public partial class Form1 : Form
{
    private readonly IRepository _repo;
    private readonly IApiClient _api;
    private BindingSource _kalkulationen;

    public Form1(IApiClient api)
    {
        InitializeComponent();
        _api = api;
        _repo = new JsonRepository("database.json");
    }

    private void ctr_NeueKalkulation_Click(object sender, EventArgs e)
    {
        var neueKalkulationForm = new Form_NeueKalkulation();

        var dialog = neueKalkulationForm.ShowDialog();
        if (dialog == DialogResult.OK)
        {
            var neueKalkulation = neueKalkulationForm.Kalkulation;
            _repo.Add(neueKalkulation);
            _kalkulationen.List.Add(neueKalkulation);
            _kalkulationen.ResetBindings(false);
        }

    }

    private void ctrl_Speichern_Click(object sender, EventArgs e)
    {
        _repo.Save();
        MessageBox.Show("Daten gespeichert.", "Vorgang", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    private void ctrl_ListeKalkulationen_SelectionChanged(object sender, EventArgs e)
    {
        var kalkulation = AuswahlEinlesen();

        if (kalkulation == null)
            return;

        OptionenAnzeigen(kalkulation);
    }

    private void OptionenAnzeigen(Dokument kalkulation)
    {
        ctrl_VersicherungsscheinAusstellen.Enabled = false;
        ctrl_AngebotAnnehmen.Enabled = false;

        switch (kalkulation.Typ)
        {
            case Dokumenttyp.Angebot:
                ctrl_AngebotAnnehmen.Enabled = true;
                break;
            case Dokumenttyp.Versicherungsschein:
                if (!kalkulation.VersicherungsscheinAusgestellt)
                    ctrl_VersicherungsscheinAusstellen.Enabled = true;
                break;
            default: throw new InvalidDataException("Unbekannter Dokumenttyp");
        }
    }

    private void ctrl_AngebotAnnehmen_Click(object sender, EventArgs e)
    {
        var kalkulation = AuswahlEinlesen();
        if (kalkulation == null)
            return;

        kalkulation.Typ = Dokumenttyp.Versicherungsschein;
        
        OptionenAnzeigen(kalkulation);
        _kalkulationen.ResetBindings(false);
    }

    private void ctrl_VersicherungsscheinAusstellen_Click(object sender, EventArgs e)
    {
        var kalkulation = AuswahlEinlesen();
        if (kalkulation == null)
            return;

        kalkulation.VersicherungsscheinAusgestellt = true;
        
        OptionenAnzeigen(kalkulation);

        MessageBox.Show("Der Versicherungsschein wurde an den Versicherungsnehmer verschickt.", "Vorgang", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    private Dokument? AuswahlEinlesen()
    {
        var rowsCount = ctrl_ListeKalkulationen.SelectedRows.Count;
        if (rowsCount == 0 || rowsCount > 1) return null;

        var zeile = ctrl_ListeKalkulationen.SelectedRows[0];
        if (zeile == null) return null;

        var kalkulation = (Dokument)zeile.DataBoundItem;
        return kalkulation;
    }

    private async void Form1_Load(object sender, EventArgs e)
    {
        var documents = await _api.DocumentsAllAsync();
        var mapped = documents.Select(doc =>
        {
            return new Dokument
            {
                Beitrag = doc.Beitrag,
                Berechnungbasis = doc.Berechnungbasis,
                Berechnungsart = (Berechnungsart)doc.Berechnungsart,
                Risiko = (Risiko)doc.Risiko,
                Typ = (Dokumenttyp)doc.Typ,
                Versicherungssumme = doc.Versicherungssumme,
                HatWebshop = doc.HatWebshop,
                InkludiereZusatzschutz = doc.InkludiereZusatzschutz,
                VersicherungsscheinAusgestellt = doc.VersicherungsscheinAusgestellt,
                ZusatzschutzAufschlag = doc.ZusatzschutzAufschlag
            };
        });
        
        _kalkulationen = new BindingSource
        {
            DataSource = mapped
        };

        ctrl_ListeKalkulationen.DataSource = _kalkulationen;

        ctrl_ListeKalkulationen.ColumnHeadersVisible = true;
        ctrl_ListeKalkulationen.AutoGenerateColumns = true;
        ctrl_ListeKalkulationen.Columns["Id"].Visible = false;
        ctrl_ListeKalkulationen.Columns["Beitrag"].DefaultCellStyle.Format = "c";
        ctrl_ListeKalkulationen.Columns["Versicherungssumme"].DefaultCellStyle.Format = "c";
        ctrl_ListeKalkulationen.AutoResizeColumns();
        ctrl_ListeKalkulationen.AutoSize = true;

        _kalkulationen.ResetBindings(false);
    }
}