
// This file has been generated by the GUI designer. Do not modify.
namespace Clonebit
{
	public partial class HomePage
	{
		private global::Gtk.Table mainTable;

		private global::Gtk.Frame duplicationFrame;

		private global::Gtk.Alignment alignement3;

		private global::Gtk.Table duplicationTable;

		private global::Gtk.Button duplicateButton;

		private global::Gtk.ScrolledWindow GtkScrolledWindow;

		private global::Gtk.TreeView stationTreeView;

		private global::Gtk.Label duplicationLabel;

		private global::Gtk.Frame fileFrame;

		private global::Gtk.Alignment alignement2;

		private global::Gtk.Table fileInfoTable;

		private global::Gtk.Label filenameDescLabel;

		private global::Gtk.Label filenameInfoLabel;

		private global::Gtk.Label fingerprintInfoLabel;

		private global::Gtk.Label fingerprintLabel;

		private global::Gtk.Label lastAccessDateDescLabel;

		private global::Gtk.Label lastAccessDateInfoLabel;

		private global::Gtk.Label lastWriteDateDescLabel;

		private global::Gtk.Label lastWriteDateInfoLabel;

		private global::Gtk.Label parentRepositoryDescLabel;

		private global::Gtk.Label parentRepositoryInfoLabel;

		private global::Gtk.Label sizeDescLabel;

		private global::Gtk.Label sizeInfoLabel;

		private global::Gtk.Label fileLabel;

		private global::Gtk.Frame selectFrame;

		private global::Gtk.Alignment alignement1;

		private global::Gtk.Table fileTable;

		private global::Gtk.Label filenameLabel;

		private global::Gtk.Label fileTypeLabel;

		private global::Gtk.Button openButton;

		private global::Gtk.ComboBox typeFileComboBox;

		private global::Gtk.Label selectLabel;

		protected virtual void Build()
		{
			global::Stetic.Gui.Initialize(this);
			// Widget Clonebit.HomePage
			global::Stetic.BinContainer.Attach(this);
			this.WidthRequest = 1280;
			this.HeightRequest = 720;
			this.Name = "Clonebit.HomePage";
			// Container child Clonebit.HomePage.Gtk.Container+ContainerChild
			this.mainTable = new global::Gtk.Table(((uint)(2)), ((uint)(2)), false);
			this.mainTable.Name = "mainTable";
			this.mainTable.RowSpacing = ((uint)(6));
			this.mainTable.ColumnSpacing = ((uint)(6));
			// Container child mainTable.Gtk.Table+TableChild
			this.duplicationFrame = new global::Gtk.Frame();
			this.duplicationFrame.Sensitive = false;
			this.duplicationFrame.Name = "duplicationFrame";
			this.duplicationFrame.ShadowType = ((global::Gtk.ShadowType)(0));
			this.duplicationFrame.LabelXalign = 0.5F;
			this.duplicationFrame.BorderWidth = ((uint)(10));
			// Container child duplicationFrame.Gtk.Container+ContainerChild
			this.alignement3 = new global::Gtk.Alignment(0F, 0F, 1F, 1F);
			this.alignement3.Name = "alignement3";
			this.alignement3.LeftPadding = ((uint)(15));
			this.alignement3.TopPadding = ((uint)(15));
			this.alignement3.RightPadding = ((uint)(15));
			this.alignement3.BottomPadding = ((uint)(15));
			// Container child alignement3.Gtk.Container+ContainerChild
			this.duplicationTable = new global::Gtk.Table(((uint)(2)), ((uint)(1)), false);
			this.duplicationTable.Name = "duplicationTable";
			this.duplicationTable.RowSpacing = ((uint)(6));
			this.duplicationTable.ColumnSpacing = ((uint)(6));
			// Container child duplicationTable.Gtk.Table+TableChild
			this.duplicateButton = new global::Gtk.Button();
			this.duplicateButton.CanFocus = true;
			this.duplicateButton.Name = "duplicateButton";
			this.duplicateButton.UseUnderline = true;
			this.duplicateButton.Label = global::Mono.Unix.Catalog.GetString("Dupliquer");
			global::Gtk.Image w1 = new global::Gtk.Image();
			w1.Pixbuf = global::Stetic.IconLoader.LoadIcon(this, "gtk-copy", global::Gtk.IconSize.Menu);
			this.duplicateButton.Image = w1;
			this.duplicationTable.Add(this.duplicateButton);
			global::Gtk.Table.TableChild w2 = ((global::Gtk.Table.TableChild)(this.duplicationTable[this.duplicateButton]));
			w2.TopAttach = ((uint)(1));
			w2.BottomAttach = ((uint)(2));
			// Container child duplicationTable.Gtk.Table+TableChild
			this.GtkScrolledWindow = new global::Gtk.ScrolledWindow();
			this.GtkScrolledWindow.Name = "GtkScrolledWindow";
			this.GtkScrolledWindow.ShadowType = ((global::Gtk.ShadowType)(1));
			// Container child GtkScrolledWindow.Gtk.Container+ContainerChild
			this.stationTreeView = new global::Gtk.TreeView();
			this.stationTreeView.CanFocus = true;
			this.stationTreeView.Name = "stationTreeView";
			this.GtkScrolledWindow.Add(this.stationTreeView);
			this.duplicationTable.Add(this.GtkScrolledWindow);
			global::Gtk.Table.TableChild w4 = ((global::Gtk.Table.TableChild)(this.duplicationTable[this.GtkScrolledWindow]));
			w4.XOptions = ((global::Gtk.AttachOptions)(4));
			this.alignement3.Add(this.duplicationTable);
			this.duplicationFrame.Add(this.alignement3);
			this.duplicationLabel = new global::Gtk.Label();
			this.duplicationLabel.Name = "duplicationLabel";
			this.duplicationLabel.LabelProp = global::Mono.Unix.Catalog.GetString("<b>Duplication</b>");
			this.duplicationLabel.UseMarkup = true;
			this.duplicationFrame.LabelWidget = this.duplicationLabel;
			this.mainTable.Add(this.duplicationFrame);
			global::Gtk.Table.TableChild w7 = ((global::Gtk.Table.TableChild)(this.mainTable[this.duplicationFrame]));
			w7.LeftAttach = ((uint)(1));
			w7.RightAttach = ((uint)(2));
			// Container child mainTable.Gtk.Table+TableChild
			this.fileFrame = new global::Gtk.Frame();
			this.fileFrame.Sensitive = false;
			this.fileFrame.Name = "fileFrame";
			this.fileFrame.ShadowType = ((global::Gtk.ShadowType)(0));
			this.fileFrame.LabelXalign = 0.5F;
			this.fileFrame.BorderWidth = ((uint)(10));
			// Container child fileFrame.Gtk.Container+ContainerChild
			this.alignement2 = new global::Gtk.Alignment(0F, 0F, 1F, 1F);
			this.alignement2.Name = "alignement2";
			this.alignement2.LeftPadding = ((uint)(15));
			this.alignement2.TopPadding = ((uint)(15));
			this.alignement2.RightPadding = ((uint)(15));
			this.alignement2.BottomPadding = ((uint)(15));
			// Container child alignement2.Gtk.Container+ContainerChild
			this.fileInfoTable = new global::Gtk.Table(((uint)(6)), ((uint)(2)), false);
			this.fileInfoTable.Name = "fileInfoTable";
			this.fileInfoTable.RowSpacing = ((uint)(6));
			this.fileInfoTable.ColumnSpacing = ((uint)(6));
			// Container child fileInfoTable.Gtk.Table+TableChild
			this.filenameDescLabel = new global::Gtk.Label();
			this.filenameDescLabel.Name = "filenameDescLabel";
			this.filenameDescLabel.Xalign = 0F;
			this.filenameDescLabel.LabelProp = global::Mono.Unix.Catalog.GetString("<b>Nom du fichier :</b>");
			this.filenameDescLabel.UseMarkup = true;
			this.fileInfoTable.Add(this.filenameDescLabel);
			global::Gtk.Table.TableChild w8 = ((global::Gtk.Table.TableChild)(this.fileInfoTable[this.filenameDescLabel]));
			w8.XOptions = ((global::Gtk.AttachOptions)(4));
			w8.YOptions = ((global::Gtk.AttachOptions)(0));
			// Container child fileInfoTable.Gtk.Table+TableChild
			this.filenameInfoLabel = new global::Gtk.Label();
			this.filenameInfoLabel.Name = "filenameInfoLabel";
			this.filenameInfoLabel.Xalign = 0F;
			this.filenameInfoLabel.UseMarkup = true;
			this.filenameInfoLabel.Selectable = true;
			this.fileInfoTable.Add(this.filenameInfoLabel);
			global::Gtk.Table.TableChild w9 = ((global::Gtk.Table.TableChild)(this.fileInfoTable[this.filenameInfoLabel]));
			w9.LeftAttach = ((uint)(1));
			w9.RightAttach = ((uint)(2));
			w9.XOptions = ((global::Gtk.AttachOptions)(4));
			w9.YOptions = ((global::Gtk.AttachOptions)(0));
			// Container child fileInfoTable.Gtk.Table+TableChild
			this.fingerprintInfoLabel = new global::Gtk.Label();
			this.fingerprintInfoLabel.Name = "fingerprintInfoLabel";
			this.fingerprintInfoLabel.Xalign = 0F;
			this.fileInfoTable.Add(this.fingerprintInfoLabel);
			global::Gtk.Table.TableChild w10 = ((global::Gtk.Table.TableChild)(this.fileInfoTable[this.fingerprintInfoLabel]));
			w10.TopAttach = ((uint)(5));
			w10.BottomAttach = ((uint)(6));
			w10.LeftAttach = ((uint)(1));
			w10.RightAttach = ((uint)(2));
			w10.XOptions = ((global::Gtk.AttachOptions)(4));
			w10.YOptions = ((global::Gtk.AttachOptions)(0));
			// Container child fileInfoTable.Gtk.Table+TableChild
			this.fingerprintLabel = new global::Gtk.Label();
			this.fingerprintLabel.Name = "fingerprintLabel";
			this.fingerprintLabel.Xalign = 0F;
			this.fingerprintLabel.LabelProp = global::Mono.Unix.Catalog.GetString("<b>Empreinte SHA256 :</b>");
			this.fingerprintLabel.UseMarkup = true;
			this.fileInfoTable.Add(this.fingerprintLabel);
			global::Gtk.Table.TableChild w11 = ((global::Gtk.Table.TableChild)(this.fileInfoTable[this.fingerprintLabel]));
			w11.TopAttach = ((uint)(5));
			w11.BottomAttach = ((uint)(6));
			w11.XOptions = ((global::Gtk.AttachOptions)(4));
			w11.YOptions = ((global::Gtk.AttachOptions)(0));
			// Container child fileInfoTable.Gtk.Table+TableChild
			this.lastAccessDateDescLabel = new global::Gtk.Label();
			this.lastAccessDateDescLabel.Name = "lastAccessDateDescLabel";
			this.lastAccessDateDescLabel.Xalign = 0F;
			this.lastAccessDateDescLabel.LabelProp = global::Mono.Unix.Catalog.GetString("<b>Date du dernier accès :</b>");
			this.lastAccessDateDescLabel.UseMarkup = true;
			this.fileInfoTable.Add(this.lastAccessDateDescLabel);
			global::Gtk.Table.TableChild w12 = ((global::Gtk.Table.TableChild)(this.fileInfoTable[this.lastAccessDateDescLabel]));
			w12.TopAttach = ((uint)(3));
			w12.BottomAttach = ((uint)(4));
			w12.XOptions = ((global::Gtk.AttachOptions)(4));
			w12.YOptions = ((global::Gtk.AttachOptions)(0));
			// Container child fileInfoTable.Gtk.Table+TableChild
			this.lastAccessDateInfoLabel = new global::Gtk.Label();
			this.lastAccessDateInfoLabel.Name = "lastAccessDateInfoLabel";
			this.lastAccessDateInfoLabel.Xalign = 0F;
			this.lastAccessDateInfoLabel.UseMarkup = true;
			this.lastAccessDateInfoLabel.Selectable = true;
			this.fileInfoTable.Add(this.lastAccessDateInfoLabel);
			global::Gtk.Table.TableChild w13 = ((global::Gtk.Table.TableChild)(this.fileInfoTable[this.lastAccessDateInfoLabel]));
			w13.TopAttach = ((uint)(3));
			w13.BottomAttach = ((uint)(4));
			w13.LeftAttach = ((uint)(1));
			w13.RightAttach = ((uint)(2));
			w13.XOptions = ((global::Gtk.AttachOptions)(4));
			w13.YOptions = ((global::Gtk.AttachOptions)(0));
			// Container child fileInfoTable.Gtk.Table+TableChild
			this.lastWriteDateDescLabel = new global::Gtk.Label();
			this.lastWriteDateDescLabel.Name = "lastWriteDateDescLabel";
			this.lastWriteDateDescLabel.Xalign = 0F;
			this.lastWriteDateDescLabel.LabelProp = global::Mono.Unix.Catalog.GetString("<b>Date de la dernière modification :</b>");
			this.lastWriteDateDescLabel.UseMarkup = true;
			this.fileInfoTable.Add(this.lastWriteDateDescLabel);
			global::Gtk.Table.TableChild w14 = ((global::Gtk.Table.TableChild)(this.fileInfoTable[this.lastWriteDateDescLabel]));
			w14.TopAttach = ((uint)(4));
			w14.BottomAttach = ((uint)(5));
			w14.XOptions = ((global::Gtk.AttachOptions)(4));
			w14.YOptions = ((global::Gtk.AttachOptions)(0));
			// Container child fileInfoTable.Gtk.Table+TableChild
			this.lastWriteDateInfoLabel = new global::Gtk.Label();
			this.lastWriteDateInfoLabel.Name = "lastWriteDateInfoLabel";
			this.lastWriteDateInfoLabel.Xalign = 0F;
			this.lastWriteDateInfoLabel.UseMarkup = true;
			this.lastWriteDateInfoLabel.Selectable = true;
			this.fileInfoTable.Add(this.lastWriteDateInfoLabel);
			global::Gtk.Table.TableChild w15 = ((global::Gtk.Table.TableChild)(this.fileInfoTable[this.lastWriteDateInfoLabel]));
			w15.TopAttach = ((uint)(4));
			w15.BottomAttach = ((uint)(5));
			w15.LeftAttach = ((uint)(1));
			w15.RightAttach = ((uint)(2));
			w15.XOptions = ((global::Gtk.AttachOptions)(4));
			w15.YOptions = ((global::Gtk.AttachOptions)(0));
			// Container child fileInfoTable.Gtk.Table+TableChild
			this.parentRepositoryDescLabel = new global::Gtk.Label();
			this.parentRepositoryDescLabel.Name = "parentRepositoryDescLabel";
			this.parentRepositoryDescLabel.Xalign = 0F;
			this.parentRepositoryDescLabel.LabelProp = global::Mono.Unix.Catalog.GetString("<b>Nom du répertoire parent :</b>");
			this.parentRepositoryDescLabel.UseMarkup = true;
			this.fileInfoTable.Add(this.parentRepositoryDescLabel);
			global::Gtk.Table.TableChild w16 = ((global::Gtk.Table.TableChild)(this.fileInfoTable[this.parentRepositoryDescLabel]));
			w16.TopAttach = ((uint)(2));
			w16.BottomAttach = ((uint)(3));
			w16.XOptions = ((global::Gtk.AttachOptions)(4));
			w16.YOptions = ((global::Gtk.AttachOptions)(0));
			// Container child fileInfoTable.Gtk.Table+TableChild
			this.parentRepositoryInfoLabel = new global::Gtk.Label();
			this.parentRepositoryInfoLabel.Name = "parentRepositoryInfoLabel";
			this.parentRepositoryInfoLabel.Xalign = 0F;
			this.parentRepositoryInfoLabel.UseMarkup = true;
			this.parentRepositoryInfoLabel.Selectable = true;
			this.fileInfoTable.Add(this.parentRepositoryInfoLabel);
			global::Gtk.Table.TableChild w17 = ((global::Gtk.Table.TableChild)(this.fileInfoTable[this.parentRepositoryInfoLabel]));
			w17.TopAttach = ((uint)(2));
			w17.BottomAttach = ((uint)(3));
			w17.LeftAttach = ((uint)(1));
			w17.RightAttach = ((uint)(2));
			w17.XOptions = ((global::Gtk.AttachOptions)(4));
			w17.YOptions = ((global::Gtk.AttachOptions)(0));
			// Container child fileInfoTable.Gtk.Table+TableChild
			this.sizeDescLabel = new global::Gtk.Label();
			this.sizeDescLabel.Name = "sizeDescLabel";
			this.sizeDescLabel.Xalign = 0F;
			this.sizeDescLabel.LabelProp = global::Mono.Unix.Catalog.GetString("<b>Taille du fichier :</b>");
			this.sizeDescLabel.UseMarkup = true;
			this.fileInfoTable.Add(this.sizeDescLabel);
			global::Gtk.Table.TableChild w18 = ((global::Gtk.Table.TableChild)(this.fileInfoTable[this.sizeDescLabel]));
			w18.TopAttach = ((uint)(1));
			w18.BottomAttach = ((uint)(2));
			w18.XOptions = ((global::Gtk.AttachOptions)(4));
			w18.YOptions = ((global::Gtk.AttachOptions)(0));
			// Container child fileInfoTable.Gtk.Table+TableChild
			this.sizeInfoLabel = new global::Gtk.Label();
			this.sizeInfoLabel.Name = "sizeInfoLabel";
			this.sizeInfoLabel.Xalign = 0F;
			this.sizeInfoLabel.UseMarkup = true;
			this.sizeInfoLabel.Selectable = true;
			this.fileInfoTable.Add(this.sizeInfoLabel);
			global::Gtk.Table.TableChild w19 = ((global::Gtk.Table.TableChild)(this.fileInfoTable[this.sizeInfoLabel]));
			w19.TopAttach = ((uint)(1));
			w19.BottomAttach = ((uint)(2));
			w19.LeftAttach = ((uint)(1));
			w19.RightAttach = ((uint)(2));
			w19.XOptions = ((global::Gtk.AttachOptions)(4));
			w19.YOptions = ((global::Gtk.AttachOptions)(0));
			this.alignement2.Add(this.fileInfoTable);
			this.fileFrame.Add(this.alignement2);
			this.fileLabel = new global::Gtk.Label();
			this.fileLabel.Name = "fileLabel";
			this.fileLabel.LabelProp = global::Mono.Unix.Catalog.GetString("<b>Informations sur le fichier à dupliquer</b>");
			this.fileLabel.UseMarkup = true;
			this.fileFrame.LabelWidget = this.fileLabel;
			this.mainTable.Add(this.fileFrame);
			global::Gtk.Table.TableChild w22 = ((global::Gtk.Table.TableChild)(this.mainTable[this.fileFrame]));
			w22.TopAttach = ((uint)(1));
			w22.BottomAttach = ((uint)(2));
			w22.RightAttach = ((uint)(2));
			w22.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child mainTable.Gtk.Table+TableChild
			this.selectFrame = new global::Gtk.Frame();
			this.selectFrame.Name = "selectFrame";
			this.selectFrame.ShadowType = ((global::Gtk.ShadowType)(0));
			this.selectFrame.LabelXalign = 0.5F;
			this.selectFrame.BorderWidth = ((uint)(10));
			// Container child selectFrame.Gtk.Container+ContainerChild
			this.alignement1 = new global::Gtk.Alignment(0F, 0F, 1F, 1F);
			this.alignement1.Name = "alignement1";
			this.alignement1.LeftPadding = ((uint)(15));
			this.alignement1.TopPadding = ((uint)(15));
			this.alignement1.RightPadding = ((uint)(15));
			this.alignement1.BottomPadding = ((uint)(15));
			// Container child alignement1.Gtk.Container+ContainerChild
			this.fileTable = new global::Gtk.Table(((uint)(2)), ((uint)(3)), false);
			this.fileTable.Name = "fileTable";
			this.fileTable.RowSpacing = ((uint)(6));
			this.fileTable.ColumnSpacing = ((uint)(6));
			// Container child fileTable.Gtk.Table+TableChild
			this.filenameLabel = new global::Gtk.Label();
			this.filenameLabel.Sensitive = false;
			this.filenameLabel.Name = "filenameLabel";
			this.filenameLabel.Xpad = 12;
			this.filenameLabel.Xalign = 0F;
			this.filenameLabel.LabelProp = global::Mono.Unix.Catalog.GetString("<i>Aucun fichier sélectionné</i>");
			this.filenameLabel.UseMarkup = true;
			this.filenameLabel.Selectable = true;
			this.fileTable.Add(this.filenameLabel);
			global::Gtk.Table.TableChild w23 = ((global::Gtk.Table.TableChild)(this.fileTable[this.filenameLabel]));
			w23.TopAttach = ((uint)(1));
			w23.BottomAttach = ((uint)(2));
			w23.LeftAttach = ((uint)(2));
			w23.RightAttach = ((uint)(3));
			w23.XOptions = ((global::Gtk.AttachOptions)(4));
			w23.YOptions = ((global::Gtk.AttachOptions)(0));
			// Container child fileTable.Gtk.Table+TableChild
			this.fileTypeLabel = new global::Gtk.Label();
			this.fileTypeLabel.Name = "fileTypeLabel";
			this.fileTypeLabel.LabelProp = global::Mono.Unix.Catalog.GetString("<b>Type de fichier à dupliquer :</b>");
			this.fileTypeLabel.UseMarkup = true;
			this.fileTypeLabel.Justify = ((global::Gtk.Justification)(1));
			this.fileTable.Add(this.fileTypeLabel);
			global::Gtk.Table.TableChild w24 = ((global::Gtk.Table.TableChild)(this.fileTable[this.fileTypeLabel]));
			w24.XOptions = ((global::Gtk.AttachOptions)(4));
			w24.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child fileTable.Gtk.Table+TableChild
			this.openButton = new global::Gtk.Button();
			this.openButton.Sensitive = false;
			this.openButton.CanFocus = true;
			this.openButton.Name = "openButton";
			this.openButton.UseUnderline = true;
			this.openButton.Label = global::Mono.Unix.Catalog.GetString("Ouvrir");
			global::Gtk.Image w25 = new global::Gtk.Image();
			w25.Pixbuf = global::Stetic.IconLoader.LoadIcon(this, "gtk-open", global::Gtk.IconSize.Menu);
			this.openButton.Image = w25;
			this.fileTable.Add(this.openButton);
			global::Gtk.Table.TableChild w26 = ((global::Gtk.Table.TableChild)(this.fileTable[this.openButton]));
			w26.TopAttach = ((uint)(1));
			w26.BottomAttach = ((uint)(2));
			w26.RightAttach = ((uint)(2));
			// Container child fileTable.Gtk.Table+TableChild
			this.typeFileComboBox = global::Gtk.ComboBox.NewText();
			this.typeFileComboBox.AppendText("");
			this.typeFileComboBox.AppendText(global::Mono.Unix.Catalog.GetString("Fichier"));
			this.typeFileComboBox.AppendText(global::Mono.Unix.Catalog.GetString("Répertoire"));
			this.typeFileComboBox.AppendText(global::Mono.Unix.Catalog.GetString("Fichier (support d\'amorçage)"));
			this.typeFileComboBox.Name = "typeFileComboBox";
			this.typeFileComboBox.Active = 0;
			this.fileTable.Add(this.typeFileComboBox);
			global::Gtk.Table.TableChild w27 = ((global::Gtk.Table.TableChild)(this.fileTable[this.typeFileComboBox]));
			w27.LeftAttach = ((uint)(1));
			w27.RightAttach = ((uint)(2));
			w27.YOptions = ((global::Gtk.AttachOptions)(4));
			this.alignement1.Add(this.fileTable);
			this.selectFrame.Add(this.alignement1);
			this.selectLabel = new global::Gtk.Label();
			this.selectLabel.Name = "selectLabel";
			this.selectLabel.LabelProp = global::Mono.Unix.Catalog.GetString("<b>Sélection du fichier à dupliquer</b>");
			this.selectLabel.UseMarkup = true;
			this.selectFrame.LabelWidget = this.selectLabel;
			this.mainTable.Add(this.selectFrame);
			this.Add(this.mainTable);
			if ((this.Child != null))
			{
				this.Child.ShowAll();
			}
			this.Hide();
			this.typeFileComboBox.Changed += new global::System.EventHandler(this.OnTypeFileComboBoxChanged);
			this.openButton.Clicked += new global::System.EventHandler(this.OnOpenButtonClicked);
			this.duplicateButton.Clicked += new global::System.EventHandler(this.OnDuplicateButtonClicked);
		}
	}
}