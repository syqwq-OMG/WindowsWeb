   M S B A M L   `   `   `  ���� K  GVoaDownloaderWpf, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:VoaDownloaderWpfVoaDownloaderWpf  !    VoaDownloaderWpf.MainWindow   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     0localclr-namespace:VoaDownloaderWpf     _ [Microsoft.Xaml.Behaviors, Version=1.1.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a3i+http://schemas.microsoft.com/xaml/behaviors    � Title$  VOA Downloader Pro��$	��700��$
��1400�� � WindowStartupLocation$ CenterScreen=�$	��600��$
��1000��   BackgroundBrush#��[   � 	Resources ��%     � DataContext $   VoaDownloaderWpf.MainViewModel  .��� $��10q� � ColumnDefinitions �� $	��260 �$	��260���� $
��Auto ��� $	��570 ��� $
��Auto ��� $��* �.+��� $��0��  ContentBackgroundBrush#��[  �� CornerRadius$ 8��$��0,0,10,0q�  InteractiveCardStyle#��[ ._�]� .+��� $��Bottom=�
������ $
��10,5q�.@��� .����   TextRegularBrush#��[ ���� *
StatusText+�� $��Right=�.��� $	��100��$��10�� � Value �� *ProgressValue+ � IsIndeterminate �� *IsBusy+�� $��15q�.+��� $��1. 选择分类��$
��Bold"�  TextPrimaryBrush#��[ �� $��0,5,0,15q����� *
Categories+ �� SelectedItem �� *SelectedCategory+�� $��2. 获取文章��$
��Bold"�#��[ �� 	 �� Orientation$	 
Horizontal=�$��0,5,0,15q�.+��� $��页码:��$��Center=�$��0,0,5,0q��� $��40��
 �� TextAlignment$
 Center=�$��Center=����� *
PageNumber+ �� UpdateSourceTrigger$ PropertyChanged=��� $��5,0,0,0q�$��Center=�.+���   SmallIconButton#��[ $	���� � ToolTip$ 增加页码������ *IncrementPageCommand+�� #��[ $	����$ 减少页码��$��0,2,0,0q����� *DecrementPageCommand+�� $��获取列表��$��10,0,0,0q�  IconFunctionButtonStyle#��[ 
 � Tag$	 ��$
��10,5q����� *FetchArticlesCommand+{���  �� Path$ IsBusy�  InverseBooleanConverter �� 	Converter# [ �� $	��0,5q��� $��3. 执行操作��$
��Bold"�#��[ $��0,15,0,5q��� $��下载选中项��#��[ $	 ��$��0,5,0,0q�$$ 下载选中的文章到本地������ *DownloadCommand+�� $��阅读选中项��#��[ $	 ��$��0,10,0,0q�$* $在阅读器中打开选中的文章������ *OpenReaderCommand+�� $��我的生词本��#��[ $	 ��$��0,10,0,0q�$ 查看已添加的生词������ *OpenVocabBookCommand+�� $��打开本地笔记��#��[ $	 ��$��0,10,0,0q�$6 0从本地文件夹打开一篇已保存的笔记������ *OpenNoteCommand+�� $��我的积累本��#��[ $	 ��$��0,10,0,0q�$! 查看收藏的好词好句������ *OpenPhraseBookCommand+�� $��1��$��5��$��Stretch=�$��Stretch=�
������ �� $��2��#��[ $ 8��$��0,0,10,0q�._�&   System.Windows.Controls.DataGrid    ModernDataGridStyle#��[  #	 ModernDataGridColumnHeaderStyle   ColumnHeaderStyle# [	  
 ModernDataGridRowStyle   RowStyle# [
   ModernDataGridCellStyle   	CellStyle# [    AutoGenerateColumns$ False��   CanUserAddRows$ False��   SelectionMode$ Extended=����� *
Articles+   Columns 4  .System.Windows.Controls.DataGridTemplateColumn  ,  &System.Windows.Controls.DataGridColumn   Width5  /System.Windows.Controls.DataGridLengthConverter$
 Auto    Header ��  d� 	IsChecked �� *DataContext.IsSelectAll+ �� RelativeSource ��  �� AncestorType DataGrid �� Mode$ TwoWay=�$ PropertyChanged=�   CellTemplate �� .R���  �� *
IsSelected+$ TwoWay=�$ PropertyChanged=�0  *System.Windows.Controls.DataGridTextColumn  $ 文章标题��$ * 1  +System.Windows.Controls.DataGridBoundColumn   Binding �� *Title+*  $Microsoft.Xaml.Behaviors.Interaction   Triggers +	  %Microsoft.Xaml.Behaviors.EventTrigger	    	  	EventName$  SelectionChanged��*
  $Microsoft.Xaml.Behaviors.TriggerBase! 
  Actions.! 2  ,Microsoft.Xaml.Behaviors.InvokeCommandAction  "   Command" �� *,*DataContext.ArticleSelectionChangedCommand+ ��  DataGrid#   CommandParameter# ��  ��  DataGrid$ SelectedItem��� $��3��$��5��$��Stretch=�$��Stretch=�
������ �� $��4��#��[ $ 8��._�� $ � RowDefinitions$ �� $
��Auto ��� $��* �.+��� $��内容预览��$��0��$
��Bold"�#��[ $��15q��� $��1��$��
15,0,15,15q�$
��Auto=�.���� $
��Wrap=�#��[ % �� 
LineHeight$% 22������ *PreviewContent+