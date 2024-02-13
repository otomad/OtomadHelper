cd docs\source
sphinx-build -b gettext . ../build/gettext
sphinx-intl update -p ../build/gettext

:: Add other languages
:: sphinx-intl update -p ../build/gettext -l zh_CN
