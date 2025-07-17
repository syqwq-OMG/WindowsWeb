from django.db import models

# Create your models here.
# accounts/models.py
from django.contrib.auth.models import AbstractUser


class CustomUser(AbstractUser):
    # 我们在 Django 自带的 User 模型基础上增加一个新的 email 字段
    # 并让它成为唯一的、不可为空的字段。
    email = models.EmailField(unique=True, blank=False, null=False)

    # 指定使用 email 字段作为用户名的认证字段
    USERNAME_FIELD = "email"

    # 在创建超级用户时，除了 email 和 password，还需要提供 username
    REQUIRED_FIELDS = ["username"]

    def __str__(self):
        return self.email
