export interface Testrun {
  name: string;
  testRunName: string;
  time: string;
  timestamp: string;
  uri: string;
  count: {
    passed: number;
    failed: number;
    skipped: number;
  };
  testCases: [];
}
