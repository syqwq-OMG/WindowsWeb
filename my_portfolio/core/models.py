# core/models.py

from django.db import models

class Project(models.Model):
    title = models.CharField(max_length=100, verbose_name="项目标题")
    description = models.TextField(verbose_name="项目描述")
    image = models.ImageField(upload_to='project_images/', verbose_name="项目主图")
    technologies = models.CharField(max_length=200, help_text="请用逗号分隔", verbose_name="使用的技术")
    project_url = models.URLField(blank=True, null=True, verbose_name="项目链接")

    class Meta:
        verbose_name = "我的项目"
        verbose_name_plural = verbose_name
        ordering = ['-id']

    def __str__(self):
        return self.title