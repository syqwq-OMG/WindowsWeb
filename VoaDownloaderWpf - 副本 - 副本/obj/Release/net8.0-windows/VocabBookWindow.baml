   M S B A M L   `   `   `  ���� K  GVoaDownloaderWpf, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:VoaDownloaderWpfVoaDownloaderWpf  &     VoaDownloaderWpf.VocabBookWindow   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     0localclr-namespace:VoaDownloaderWpf     _ [Microsoft.Xaml.Behaviors, Version=1.1.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a3i+http://schemas.microsoft.com/xaml/behaviors    � Title$  我的生词本��$	��600��$	��800�� � WindowStartupLocation$ CenterScreen=�   BackgroundBrush#��[   � 	Resources ��%    .��� $��15q� � RowDefinitions �� $
��Auto ��� $��* ��� $
��Auto �.+��� $��0��  ContentBackgroundBrush#��[  �� CornerRadius$ 4�� �� Padding$ 10q�$��0,0,0,10q�._��� $��显示已学会的单词��$��Center=�#��[B�� d� 	IsChecked �� *ShowLearned+&   System.Windows.Controls.DataGrid  $��1��  ModernDataGridStyle#��[  # ModernDataGridColumnHeaderStyle   ColumnHeaderStyle# [   ModernDataGridRowStyle   RowStyle# [   ModernDataGridCellStyle	   	CellStyle#	 [ 
   AutoGenerateColumns$
 False��   CanUserAddRows$ False��   HeadersVisibility$ Column=�   SelectionMode$ Extended=����� *VocabEntries+   Columns 4  .System.Windows.Controls.DataGridTemplateColumn  ,  &System.Windows.Controls.DataGridColumn   Width5  /System.Windows.Controls.DataGridLengthConverter$
 Auto    Header ��  �� *DataContext.IsAllSelected+ �� RelativeSource ��  �� AncestorType DataGrid �� Mode$ TwoWay=� �� UpdateSourceTrigger$ PropertyChanged=�   CellTemplate �� .R��� $��Center=�$��Center=� �� *
IsSelected+$ PropertyChanged=����� *%#DataContext.SelectionChangedCommand+ ��  DataGrid0  *System.Windows.Controls.DataGridTextColumn  $ 状态��$
 Auto    
FontWeight$
 Bold"�   
Foreground
 � � �1  +System.Windows.Controls.DataGridBoundColumn   Binding �� *	IsLearned+  LearnedStatusConverter �� 	Converter# [   $ 单词��$ *  �� *Word+  $ 释义��$ 3*  �� *
Definition+  $ 学习次数��$
 Auto  �� *LookupCount+  $ 添加日期��$
 Auto  �� *	DateAdded+ �� StringFormat$ 
yyyy-MM-dd���� $��2��#��[ $ 10q�$��0,10,0,0q�  BorderBrush#��[ $��0,1,0,0q�._��  � ColumnDefinitions �� $��* ��� $
��Auto �.+��� $��0�� �� Orientation$ 
Horizontal=�.+��� $��标记为“已学会”��  IconFunctionButtonStyle#��[ 
 � Tag$	 ������ *MarkSelectedAsLearnedCommand+y��� *ShowLearned+ '	 #InverseBooleanToVisibilityConverter# [	 �� $��标记为“未学会”��#��[ $	 ������ * MarkSelectedAsForgottenCommand+y��� *ShowLearned+  
 BooleanToVisibilityConverter# [
 �� $��彻底删除�� ! DangerIconFunctionButtonStyle#��[ $	 ��$��20,0,0,0q����� *DeleteSelectedCommand+�� $��1��$ 
Horizontal=�.+���   ReviewButtonStyle#��[  � ToolTip$* $从所有生词中随机开始复习������ *FreeReviewCommand+.���� $ 
Horizontal=�.+��� $	����  IconFont#��[ $��Center=�$��0,0,6,0q��� $��自由复习���� 
��� E��$
��Bold"����� *AllUnlearnedWordsCount+$  ({0})��y��� *AllUnlearnedWordsCount+  IntToVisibilityConverter# [ �� #��[ $��10,0,0,0q�$E ?根据艾宾浩斯遗忘曲线开始复习今天到期的单词������ *EbbinghausReviewCommand+.���� $ 
Horizontal=�.+��� $	����#��[ $��Center=�$��0,0,6,0q��� $��今日计划���� 
��� E��$
��Bold"����� *WordsToReviewCount+$  ({0})��y��� *WordsToReviewCount+# [ 