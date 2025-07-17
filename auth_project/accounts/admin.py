# accounts/admin.py

from django.contrib import admin
from .models import CustomUser # 导入你的用户模型

# 将 CustomUser 模型注册到 Django Admin 后台
admin.site.register(CustomUser)