# CookieReader

Read Google Chrome Cookie, filter using text content and output to `.txt` file

# Usage
## Get help
```bash
$ cookiereader --help
Usage: cookiereader [options] [command]

Options:
  --help  Show help information

Commands:
  read    Read a Google Chrome cookie file

Run 'cookiereader [command] --help' for more information about a command.
```
## Read help
```bash
$ cookiereader read --help
Read a Google Chrome cookie file

Usage: cookiereader read [options]

Options:
  --help                            Show help information
  -f|--filter-text <string filter>  filer result by some text
  -o|--output-file <output file>    output file
```

## Read your cookies
```bash
$ cookiereader read -f google -o output.txt
# 1. Will read cookies and output the cookies that contain 'google' in it's values
# 2. Output the result to output.txt
```

