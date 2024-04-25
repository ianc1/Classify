# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [2.0.0-prerelease.1] - 2024-04-25

### Changed

- The Microsoft and Newtonsoft JSON serializers will nolonger redact the sensitive data
  types by default. This feature provided minimal benefit and complicated configuration.

### Added

- Support for .Net 8.

### Fixed

- The `PII` and `Secret` types now implement `IComparable` so that they can be sorted.


## [1.0.0] - 2021-05-27

- Initial release