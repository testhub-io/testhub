export interface Testrun {
  Name: string;
  TestRunName: string;
  Time: string;
  Timestamp: string;
  uri: string;
  Count: {
    Passed: number;
    Failed: number;
    Skipped: number;
  };
}
