   M S B A M L   `   `   `  ����K  GVoaDownloaderWpf, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:VoaDownloaderWpfVoaDownloaderWpf  5   	   '    !VoaDownloaderWpf.PhraseBookWindow   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   0localclr-namespace:VoaDownloaderWpf     5   	   _ [Microsoft.Xaml.Behaviors, Version=1.1.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a3i+http://schemas.microsoft.com/xaml/behaviors  5   	     � Title$  我的积累本��5	   	   $	��600��6   $	��900��6$    � WindowStartupLocation$ CenterScreen=�5
   	      BackgroundBrush#��[  5   	    � DataContext 5      *   $VoaDownloaderWpf.PhraseBookViewModel  5   
   .��5      � $��15q�6    � RowDefinitions 5   
   �� 5      $��* �6   �� 5      $
��Auto �6   .+�5   
   &   System.Windows.Controls.DataGrid  $��0��6     ModernDataGridStyle#��[ 5       # ModernDataGridColumnHeaderStyle   ColumnHeaderStyle# [ 5        ModernDataGridRowStyle   RowStyle# [ 5        ModernDataGridCellStyle   	CellStyle# [ 5         AutoGenerateColumns$ False��5      ��5       �� *PhraseEntries+   Columns 5      4  .System.Windows.Controls.DataGridTemplateColumn  5      ,  &System.Windows.Controls.DataGridColumn	   Width5  /System.Windows.Controls.DataGridLengthConverter$
	 Auto 6)   
   Header
 5      �� 5    
    d� 	IsChecked 6   �� *DataContext.IsAllSelected+ �� RelativeSource ��  �� AncestorType DataGrid �� Mode$ TwoWay=� �� UpdateSourceTrigger$ PropertyChanged=�   CellTemplate 5%      �� 5&   
   .R�5'      �� $��Center=�5(      $��Center=�5)       5'      �� *
IsSelected+$ PropertyChanged=���5*      �� *%#DataContext.SelectionChangedCommand+ ��  DataGrid5+      5,   .     5.      $
 内容��6-   $	 3* 69    5/      �� 50      .R�51      �� $
��Wrap=�6A   $��Center=�6U   ��6(   �� *	Content+6U   52      54      0  *System.Windows.Controls.DataGridTextColumn  55      $
 来源文章��6%   $	 * 6Z   1  +System.Windows.Controls.DataGridBoundColumn   Binding 63   �� *SourceArticleTitle+6Z     56      $
 收藏日期��6%   $
	 Auto 6l    63   �� *	DateAdded+ �� StringFormat$ 
yyyy-MM-dd��6l   58      �� 5:   
   $��1��6     ContentBackgroundBrush#��[ 6    �� Padding$ 10q�6S   $��0,10,0,0q�6`     BorderBrush#��[ 5;      $��0,1,0,0q�6<   ._�5<      �  � ColumnDefinitions 5=      �� 5>      $
��Auto �6'   �� 5@      $
��Auto �6'   .+�5C      �� $��0��6   $��删除选中项��6)    ! DangerIconFunctionButtonStyle#��[ 5D      
 � Tag$	 ��6H   ��5C   9   �� *DeleteSelectedCommand+5D   H   �� 5F      $��1��6   $��导出积累本��6)     IconFunctionButtonStyle#��[ 5G      $	 ��6B   ��5F   9   �� *ExportCommand+5G   B   5H      5I      5J      5K      