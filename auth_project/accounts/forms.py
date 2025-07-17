# accounts/forms.py
from django import forms
from django.contrib.auth.forms import UserCreationForm
from .models import CustomUser

class CustomUserCreationForm(UserCreationForm):
    class Meta(UserCreationForm.Meta):
        model = CustomUser
        # 只需要指定我们自定义模型中需要用户填写的字段
        # UserCreationForm 会自动为我们处理好密码和密码确认字段
        fields = ('username', 'email')
        # 在这里添加中文翻译
        labels = {
            'username': '用户名',
            'email': '邮箱地址',
        }
        help_texts = {
            'username': '必填。150个字符或者更少。只能包含字母，数字和@/./+/-/_这些字符。',
        }