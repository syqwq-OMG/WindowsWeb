   M S B A M L   `   `   `  ���� K  GVoaDownloaderWpf, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:VoaDownloaderWpfVoaDownloaderWpf  $    VoaDownloaderWpf.ReadingWindow   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     _ [Microsoft.Xaml.Behaviors, Version=1.1.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a3i+http://schemas.microsoft.com/xaml/behaviors  *localclr-namespace:VoaDownloaderWpf  $	��760��$
��1214��  � WindowStartupLocation$  CenterScreen=�$	��600��$	��500�����#F5F5F5 � Title �� *ArticleTitle+ � 	Resources    ActionButtonStyle �� 
TargetType �� Property �� Value �� 
TargetType ��Name �� CornerRadius	 �� Triggers
 P� Property P� Value �� 
TargetName  PlayPauseButtonStyle A�Name �� Triggers �� Binding �� Value  NavigationButtonStyle  MarkingToolButtonStyle  AudioSliderStyle ]�Name ]� Thumb ]� DecreaseRepeatButton �� DecreaseLarge��%�
  &	        &	 �    &	 �    &	 �    &	 p    �� " ��.���  � ��$
 10,5q���  � ��$ 0,0,10,0q���  � ��$ 14(���  �@�Cursor$
 Hand����  � �� �#F5F7FA��  � �� �#DCDFE6��  � ��$ 1q���  � �� �� " ��.R��� $ border��#��z��#��z��#��z��$ 4��._��� $��Center=�$��Center=�	 P� 
 �@J�IsMouseOver$
 True��. ��� $ border�� � �� �#EAF4FC�� $ border�� � �� �#007ACCP� 
 � ��$
 True��. ��� $ border�� � �� �#D4E9F9�� " ��.���  � ��$ 50����  � ��$ 50����  � ��$ 22(���  � ��$ Segoe MDL2 Assets)���  � �� �#0078D7��  � ��
 �������  � ��$ 0q���  �@�Cursor$
 Hand����  � �� �� " ��.R�� .+�A� $ backgroundEllipse��#��z���� $��Center=�$��Center=�	 P� 
 �@J�IsMouseOver$
 True��. ��� $ backgroundEllipse�� � �� �#005A9EP� 
 � ��$
 True��. ��� $ backgroundEllipse�� � �� �#003D6AP� 
 � {�$ False��. ���  �@J�Opacity$	 0.5Q� ��  �� *	IsPlaying+	 False.a���  � ��$	 ����  �� *	IsPlaying+ True.a���  � ��$	 ���� " ��.���  � ��$ 40����  � ��$ 40����  � ��$ Segoe MDL2 Assets)���  � ��$ 18(���  � ��
 ���� ��  � �� �#555555��  � ��$ 1q���  � �� �#DDDDDD��  �@�Cursor$
 Hand����  � �� �� " ��.R��� $ border��#��z��#��z��#��z��$ 20��._��� $��Center=�$��Center=�	 P� 
 �@J�IsMouseOver$
 True��. ��� $ border�� � �� �#E0E0E0P� 
 � {�$ False��. ���  �@J�Opacity$	 0.3Q��� " 	�.���  � ��$
 10,5q���  � ��$ 0,0,10,0q���  � ��$ 14(���  �@�Cursor$
 Hand����  � �� �� " 	�.R��� $ border��
������ ���#CCCCCC$��1q�$ 4��._��� $��Center=�$��Center=�	 P� 
 �@d�	IsChecked$
 TrueY�. ��� $ border�� � �� �#007ACC�� $ border�� � �� �#007ACC��  � ��
 �����P� 
 �@J�IsMouseOver$
 True��. ��� $ border�� � �� �#007ACC�� " ��.���  � ��$ 20����  � �� �� " ��.R�� $��Center=�.+��� $ TrackBackground��$��4�����#E0E0E0$ 2���� $ PART_SelectionRange��$��4��$
��Left=���Z� ]� $ 
PART_Track�� n� $��16��$��16�����#0078D7
�������$��2q����� " n�.R�� .+�A� #��z��#��z��#��z�� �� #��Z ���� .R��� $ TrackDecrease��$��4�����#0078D7$ 2,0,0,2��	 P� 
 � {�$ False��. ���  �@J�Opacity$	 0.5Q�*  $Microsoft.Xaml.Behaviors.Interaction   Triggers +  %Microsoft.Xaml.Behaviors.EventTrigger     	EventName$ Closing��*  $Microsoft.Xaml.Behaviors.TriggerBase   Actions. 2  ,Microsoft.Xaml.Behaviors.InvokeCommandAction     Command �� *WindowClosingCommand+.��� $��20q� � ColumnDefinitions �� $��2* �$	��400���� $
��Auto ��� $	��330 �$	��250��.+�� $��0�� � RowDefinitions �� $
��Auto ��� $
��Auto ��� $��* �.+��� $��0��  ContentBackgroundBrush#��[ $ 8�� �� Padding$ 20q�$��0,0,0,10q�._��� .+�� $��0,0,0,15q� �� $
��Auto ��� $��* ��� $
��Auto �.+��� $��0��$	����#��[ ���� *PreviousArticleCommand+�� $��1��$��24(�$
��Bold"�$
��Wrap=�  TextPrimaryBrush#��[  �� TextAlignment$ Center=�$��Center=�$
��20,0q����� *ArticleTitle+�� $��2��$	����#��[ ���� *NextArticleCommand+� $��Center=� �� $
��Auto ��� $
��Auto ��� $��* ��� $
��Auto �.+��� $��0��#��[ ���� *PlayPauseCommand+{���  �� Path$ IsBusy�  InverseBooleanConverter �� 	Converter# [ �� $��1��$
��15,0q����� *AvailableSpeedRatios+���� .R���  �� Orientation$ 
Horizontal=����� .R�	�   	� 	GroupName$  SpeedRatioGroup��  TabRadioButtonStyle#��[ ���� *DisplayName+���� *DataContext.SetSpeedCommand+! �� RelativeSource! �� " �� AncestorType" ItemsControl���� *Value+�� $��2��$
��15,0q�#��[ # � Maximum# �� *MaximumPosition+$ � Value$ �� *CurrentPosition+% �� Mode$% TwoWay=�{��� $ IsBusy�# [    $  PreviewMouseLeftButtonDown��.    �� *SliderDragStartedCommand+  $ PreviewMouseLeftButtonUp��.    �� *SliderDragCompletedCommand+&   CommandParameter& �� ! �� 
" Slider$ Value��� $��3��$��Center=� 	 TextRegularBrush#��[	 $��Italic$����� *
StatusText+�� $��1��
�������$ 8��$ 10q�$��0,0,0,10q�._��� $ 
Horizontal=�.+��� $��标记工具:��$
��Bold"�$��Center=�$��5,0,15,0q��� $��高亮��' � ToolTip$' 	荧光笔�� 
 IconFunctionButtonStyle#��[
 
( � Tag$	( ������ *HighlightCommand+�� $��划线��$' 	下划线��#��[
 $	( ������ *UnderlineCommand+�� $��橡皮��$' 清除格式��#��[
 $	( ������ *EraserCommand+�� $��收藏��$'' !将选中的句子加入积累本��#��[
 $	( ��$��10,0,0,0q����� *AddToPhraseBookCommand+�� $
��10,0q��� $��保存笔记��$-' '保存在线文章或更新本地笔记��#��[
 $	( ������ *SaveOrUpdateNotesCommand+�� $��2��
�������$ 8��) J� Effect) 3  -System.Windows.Media.Effects.DropShadowEffect  *   Color$* #CCCCCC��+   
BlurRadius$+ 10Q�,   ShadowDepth$, 1Q�-   Opacity$	- 0.5Q�._��� -   . ��Name$. ArticleRichTextBox��$��5q�$��15q�$��18(�$��Georgia)�$��0q�"/ �� VerticalScrollBarVisibility$
/ Auto=�0 �� 
IsReadOnly$
0 True��. �/� �� $��1��$��5��$��Stretch=�
������ � $��2��$��10,0,0,0q� �� $
��Auto �$	��200���� $
��Auto ��� $��* �$	��200��.+��� $��0��#��[ $ 8��._�]� .+��� $��单词释义��$	��Top=�$
��Bold"�#��[ $��
15,15,15,5q��� $	��Top=��� $��
15,0,15,15q�$
��Auto=�.���� .+��� $
��Bold"�$��22(�  PrimaryBrush#��[ $
��Wrap=����� *SelectedWord+�� $
��Wrap=�1 �� 
LineHeight$1 24��$��16(�$��0,10,0,0q����� *SelectedWordDefinition+�� $ 
Horizontal=�$��0,20,0,0q�.+��� $��加入生词本��$$' 将当前单词加入生词本��#��[
 $	( ��$
��Left=����� *AddToVocabCommand+{��� *CanAddToVocab+�� $��查看生词本��$' 打开生词本窗口��#��[
 $	( ��$��10,0,0,0q����� *ViewVocabBookCommand+�� $��1��$��5��$��Stretch=�
������ 2 �� ResizeDirection$
2 Rows=��� $��2��#��[ $ 8��$��0,10,0,0q�._��  �� $
��Auto ��� $��* ��� $
��Auto �.+��� $��与AI助教对话��$��0��$
��Bold"�#��[ $��
15,10,15,5q��� $��0��$��Bottom=��� -   3 ��Name$3 ChatScrollViewer��$��1��$
��Auto=�$��10q�.���� ���� *ChatHistory+���� .R��� ���� *IsUserMessage+  ChatBubbleStyleConverter# [ ._��� .+��� $
��Wrap=����� *	Content+���� *IsUserMessage+  ChatForegroundConverter# [ � $��1��4 � IsIndeterminate$
4 True��$��5��$	��Top=�y��� *IsAiThinking+   BooleanToVisibilityConverter# [ � $��2��$��10q� �� $��* ��� $
��Auto �.+��� -   5 ��Name$5 UserInputTextBox��$��14(�$	��8,5q����#CCCCCC$��Center=�6 �� AcceptsReturn$
6 True��7 �� TextWrapping$
7 Wrap=�$��35��$	��100��$
/ Auto=����� *	UserInput+8 �� UpdateSourceTrigger$8 PropertyChanged=�{��� *IsAiThinking+# [ �� $��1��  SendButtonStyle#��[ $' 发送消息��$��10,0,0,0q�$��Bottom=����� *SendMessageCommand+