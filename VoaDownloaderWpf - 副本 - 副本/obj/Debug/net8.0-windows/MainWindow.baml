   M S B A M L   `   `   `  ����K  GVoaDownloaderWpf, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:VoaDownloaderWpfVoaDownloaderWpf  5   	   !    VoaDownloaderWpf.MainWindow   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   0localclr-namespace:VoaDownloaderWpf     5   	   _ [Microsoft.Xaml.Behaviors, Version=1.1.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a3i+http://schemas.microsoft.com/xaml/behaviors  5   	     � Title$  VOA Downloader Pro��5	   	   $	��700��6$   $
��1400��61    � WindowStartupLocation$ CenterScreen=�5
   	   $	��600��6.   $
��1000��6>      BackgroundBrush#��[  5   	    � 	Resources 5      ��%    5       � DataContext 5      $   VoaDownloaderWpf.MainViewModel  5   
   .��5      � $��10q�6    � ColumnDefinitions 5   
   �� 5      $	��260 �6   $	��260��6+   �� 5      $
��Auto �6   �� 5      $	��570 �6   �� 5      $
��Auto �6   �� 5      $��* �6   .+�5   
   �� $��0��6     ContentBackgroundBrush#��[ 6!    �� CornerRadius$ 8��6V   $��0,0,10,0q�6g     InteractiveCardStyle#��[ 6y   ._�5      ]� .+�5      �� $��Bottom=�6   
������ $
��10,5q�6M   .@�5       �� .��5!      ��   TextRegularBrush#��[ 6@   ��6$   �� *
StatusText+6@   5"      �� 5#      $��Right=�6$   .��5$      � $	��100��6i   $��10��6u    � Value 6&   �� *ProgressValue+ � IsIndeterminate 6F   �� *IsBusy+6u   5%      5&      �� 5(      $��15q�6   .+�5)      �� $��1. 选择分类��6    $
��Bold"�6/     TextPrimaryBrush#��[ 6A   �� 5*      $��0,5,0,15q�6l   ��6   �� *
Categories+ �� SelectedItem 6B   �� *SelectedCategory+6l   �� 5,      $��2. 获取文章��6    $
��Bold"�6/   #��[ 6A   �� 5-      	 �� Orientation$	 
Horizontal=�6!   $��0,5,0,15q�6:   .+�5.      �� $��页码:��6$   $��Center=�6/   $��0,0,5,0q�6J   �� 5/      $��40��6c   
 �� TextAlignment$
 Center=�6n   $��Center=�6�   ��6"   �� *
PageNumber+ �� UpdateSourceTrigger$ PropertyChanged=�6�   �� 51      $��5,0,0,0q�6%   $��Center=�66   .+�52      ��   SmallIconButton#��[ 6N   $	����6w    � ToolTip$ 增加页码��6�   ��6%   �� *IncrementPageCommand+6�   �� 53      #��[ 6N   $	����6w   $ 减少页码��6�   $��0,2,0,0q�6�   ��6%   �� *DecrementPageCommand+6�   54      �� 56      $��获取列表��6!   $��10,0,0,0q�59   !     IconFunctionButtonStyle#��[ 5:   !   
 � Tag$	 ��5;   !   $
��10,5q�5<   !   ��57   !   �� *FetchArticlesCommand+{�58   !   ��  �� Path$ IsBusy�  InverseBooleanConverter �� 	Converter# [ 5<   !   5=      �� 5?      $	��0,5q�6    �� 5A      $��3. 执行操作��6    $
��Bold"�6/   #��[ 6A   $��0,15,0,5q�6p   �� 5C      $��下载选中项��6   #��[ 6Q   $	 ��6�   $��0,5,0,0q�6�   $$ 下载选中的文章到本地��6�   ��6-   �� *DownloadCommand+6�   �� 5D      $��阅读选中项��6   #��[ 6S   $	 ��6�   $��0,10,0,0q�6�   $* $在阅读器中打开选中的文章��6�   ��6-   �� *OpenReaderCommand+6�   �� 5E      $��我的生词本��6   #��[ 6V   $	 ��6�   $��0,10,0,0q�6�   $ 查看已添加的生词��6�   ��6-   �� *OpenVocabBookCommand+6�   �� 5F      $��打开本地笔记��6   #��[ 5H      $	 ��5I      $��0,10,0,0q�5J      $6 0从本地文件夹打开一篇已保存的笔记��5K      ��5G      �� *OpenNoteCommand+5K      �� 5M      $��我的积累本��6   #��[ 5O      $	 ��5P      $��0,10,0,0q�5Q      $! 查看收藏的好词好句��5R      ��5N      �� *OpenPhraseBookCommand+5R      5S      5T      5U      �� 5V   
   $��1��6   $��5��6'   $��Stretch=�61   $��Stretch=�6O   
������ 6k   �� 5X   
   $��2��6   #��[ 6!   $ 8��6V   $��0,0,10,0q�6g   ._�5Y      &   System.Windows.Controls.DataGrid    ModernDataGridStyle#��[ 5Z       #	 ModernDataGridColumnHeaderStyle   ColumnHeaderStyle# [	 5[       
 ModernDataGridRowStyle   RowStyle# [
 5\        ModernDataGridCellStyle   	CellStyle# [ 5]         AutoGenerateColumns$ False��5^         CanUserAddRows$ False��63      SelectionMode$ Extended=�6J   ��5Y      �� *
Articles+   Columns 5_      4  .System.Windows.Controls.DataGridTemplateColumn  5`      ,  &System.Windows.Controls.DataGridColumn   Width5  /System.Windows.Controls.DataGridLengthConverter$
 Auto 6-      Header 5a      �� 5b       d� 	IsChecked 6'   �� *DataContext.IsSelectAll+ �� RelativeSource ��  �� AncestorType DataGrid �� Mode$ TwoWay=�$ PropertyChanged=�   CellTemplate 5d      �� 5e      .R�5f   "   ��  6+   �� *
IsSelected+$ TwoWay=�$ PropertyChanged=�5g      5i      0  *System.Windows.Controls.DataGridTextColumn  5j      $ 文章标题��6)   $ * 6Q   1  +System.Windows.Controls.DataGridBoundColumn   Binding 67   �� *Title+6Q   *  $Microsoft.Xaml.Behaviors.Interaction   Triggers 5l      +	  %Microsoft.Xaml.Behaviors.EventTrigger	  5m        	  	EventName$  SelectionChanged��6%   *
  $Microsoft.Xaml.Behaviors.TriggerBase! 
  Actions.! 5n      2  ,Microsoft.Xaml.Behaviors.InvokeCommandAction  "   Command" 5o      �� *,*DataContext.ArticleSelectionChangedCommand+ ��  DataGrid#   CommandParameter# 5p      ��  ��  DataGrid$ SelectedItem�5q      5s      5t      �� 5u   
   $��3��6   $��5��6'   $��Stretch=�61   $��Stretch=�6O   
������ 6k   �� 5w   
   $��4��6   #��[ 6!   $ 8��6V   ._�5x      � $ � RowDefinitions$ 5y      �� 5z      $
��Auto �6$   �� 5{      $��* �6$   .+�5}      �� $��内容预览��6   $��0��6(   $
��Bold"�65   #��[ 6G   $��15q�6v   �� 5~      $��1��6   $��
15,0,15,15q�6,   $
��Auto=�6@   .��5      �� $
��Wrap=�6@   #��[ 6T   % �� 
LineHeight$% 22��6�   ��6    �� *PreviewContent+6�   5�      5�      5�      5�      5�      