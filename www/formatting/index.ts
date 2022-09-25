const currencyFormatter = Intl.NumberFormat("fi-FI", {
  style: "currency",
  currency: "EUR",
});

export function formatEuro(amount: number) {
  return currencyFormatter.format(amount);
}

export function formatTransactionDate(date: Date) {
  return date.toLocaleDateString();
}
