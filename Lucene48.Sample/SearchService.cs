﻿using System;
using System.Collections.Generic;
using System.Linq;
using Lucene.Net.Analysis;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.Store;
using Lucene.Net.Util;

namespace Lucene48.Sample
{
    public class SearchService
    {
        protected internal void BuildIndex()
        {
            Analyzer analyzer = new CaseInsensitiveWhiteSpaceAnalyser();
            var writer = new IndexWriter(new RAMDirectory(), new IndexWriterConfig(LuceneVersion.LUCENE_48, analyzer));

            var products = GenerateProducts();

            var totalOrderCount = Convert.ToSingle(products.Sum(x => x.OrderCount));

            foreach (var product in products)
            {
                AddDocToIndex(writer, product, totalOrderCount);
            }

            writer.Flush(true, true);
            writer.Commit();
            writer.Dispose();
        }

        protected internal List<ProductSearchDto> GenerateProducts()
        {
            return new List<ProductSearchDto>
            {
                new ProductSearchDto{ProductId = "001", Description = "",  LargeImageUrl = "", OrderCount =  2323,
                    ProductName = "", ProductKeywords = new List<ProductKeyword>{ new ProductKeyword { Keyword = ""} }}
            };
        }

        private static void AddDocToIndex(IndexWriter writer, ProductSearchDto product, float totalOrderCount)
        {
            var productOrderCountWeighting = Convert.ToSingle(product.OrderCount) / totalOrderCount * 100.0f;

            var productIdField = new Field("ProductId", product.ProductId, new FieldType
            {
                IsStored = true,
                IsIndexed = true,
                IsTokenized = false
            })
            {
                Boost = 10.0f * productOrderCountWeighting
            };

            var productNameField = new Field("ProductName", product.ProductName, new FieldType
            {
                IsStored = true,
                IsIndexed = true,
                IsTokenized = true
            });
            productNameField.Boost = 8.0f * productOrderCountWeighting;

            var productDescriptionField = new Field("Description", !string.IsNullOrEmpty(product.Description) ? product.Description.ToLower() : "", new FieldType
            {
                IsStored = true,
                IsIndexed = false,
                IsTokenized = false
            });

            var productLargeImageUrlField = new Field("LargeImageUrl", product.LargeImageUrl, new FieldType
            {
                IsStored = true,
                IsIndexed = false,
                IsTokenized = false
            });

            var keywordFields = new List<Field>();
            foreach (var keyword in product.ProductKeywords)
            {
                var keywordField = new Field("Keywords", keyword.Keyword, new FieldType
                {
                    IsStored = true,
                    IsIndexed = true,
                    IsTokenized = false
                });
                keywordFields.Add(keywordField);
            }

            var doc = new Document();
            doc.Fields.Add(productIdField);
            doc.Fields.Add(productNameField);
            doc.Fields.Add(productDescriptionField);
            doc.Fields.Add(productLargeImageUrlField);

            foreach (var keywordField in keywordFields)
            {
                doc.Fields.Add(keywordField);
            }

            if (writer.Config.OpenMode == OpenMode.CREATE)
                writer.AddDocument(doc);
            else
                writer.UpdateDocument(new Term("ProductId", product.ProductId), doc);
        }
    }
}