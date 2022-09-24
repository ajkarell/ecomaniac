<template>
  <div class="max-w-3xl px-4 py-2 rounded-lg shadow-md bg-gray-50">
    <table class="table-auto">
      <thead>
        <tr>
          <th>Date</th>
          <th>Description</th>
          <th>Amount</th>
        </tr>
      </thead>
      <tbody>
        <tr
          v-for="transaction in transactions"
          :key="transaction.id"
          class="cursor-pointer hover:bg-slate-100"
          @click="gotoTransactionDetails(transaction.id)"
        >
          <td>{{ formatTransactionDate(transaction.date) }}</td>
          <td>{{ transaction.description }}</td>
          <td
            class="font-bold"
            :class="{
              'text-green-600': transaction.amount > 0,
              'text-red-600': transaction.amount < 0,
            }"
          >
            {{ formatEuro(transaction.amount) }}
          </td>
        </tr>
      </tbody>
    </table>
  </div>
</template>

<script setup lang="ts">
import { createApi } from "~~/api";
import { Transaction } from "~~/models/transaction";
import { formatEuro, formatTransactionDate } from "~~/formatting";

const { getTransactions } = createApi();

const transactions = ref<Transaction[] | null>(null);

function gotoTransactionDetails(id: number) {
  navigateTo(`/transactions/${id}`);
}

onMounted(async () => {
  const result = await getTransactions();
  if (result.ok) {
    transactions.value = result.value;
  }
});
</script>
