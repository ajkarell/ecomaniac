export interface Transaction {
  id: number;
  date: Date;
  amount: number;
  description: string;
}

export function isTransactionValid(transaction: Transaction) {
  const today = new Date();
  today.setUTCHours(0,0,0,0);

  return transaction.amount !== null &&
  transaction.amount !== 0 &&
  transaction.description.length !== 0 &&
  transaction.date <= today;
}
