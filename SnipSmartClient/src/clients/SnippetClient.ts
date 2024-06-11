import type { SnippetModel } from '@/models/SnippetModel'
import { BaseClient } from './BaseClient'
import { snippets } from '@codemirror/lang-javascript'

export class SnippetClient extends BaseClient {
  public async AddNewSnippet(
    Link: string,
    ContentType: string,
    ContentSubType: string,
    Content: string,
    Description: string
  ): Promise<string> {
    try {
      const resp = await this.GetAxios().post(this.baseUrl + '/snippet/', {
        SnippetID: '',
        CollectionID: '',
        Link: Link,
        ContentType: ContentType,
        ContentSubType: ContentSubType,
        Content: Content,
        Description: Description
      })
      return resp.data
    } catch (error: any) {
      if (error.response) {
        throw new Error(`Failed to fetch data: "${error.response.data}"`)
      }
      throw new Error(`Failed to fetch data: ${error}`)
    }
  }
  public async RemoveSnippet(SnippetID: string): Promise<string> {
    try {
      const resp = await this.GetAxios().delete(this.baseUrl + '/snippet/' + SnippetID, {})
      return resp.data
    } catch (error: any) {
      if (error.response) {
        throw new Error(`Failed to fetch data: "${error.response.data}"`)
      }
      throw new Error(`Failed to fetch data: ${error}`)
    }
  }
  public async EditSnippet(
    Link: string,
    ContentType: string,
    ContentSubType: string,
    Content: string,
    Description: string,
    SnippetID: string,
    CollectionID: string
  ): Promise<string> {
    try {
      const resp = await this.GetAxios().put(this.baseUrl + '/snippet/', {
        SnippetID: SnippetID,
        CollectionID: CollectionID,
        Link: Link,
        ContentType: ContentType,
        ContentSubType: ContentSubType,
        Content: Content,
        Description: Description
      })
      return resp.data
    } catch (error: any) {
      if (error.response) {
        throw new Error(`Failed to fetch data: "${error.response.data}"`)
      }
      throw new Error(`Failed to fetch data: ${error}`)
    }
  }
  public async GetAllSnippets(): Promise<Array<SnippetModel>> {
    try {
      const resp = await this.GetAxios().get(this.baseUrl + '/snippet/', {})
      return resp.data
    } catch (error: any) {
      if (error.response) {
        throw new Error(`Failed to fetch data: "${error.response.data}"`)
      }
      throw new Error(`Failed to fetch data: ${error}`)
    }
  }
  public async GetSnippetsByContentType(
    type: string,
    subtype: string
  ): Promise<Array<SnippetModel>> {
    try {
      const resp = await this.GetAxios().get(this.baseUrl + '/snippet/bycontent_type/' + type, {})
      if (type == 'ALL') {
        return resp.data
      }
      console.log('python')
      console.log(subtype)
      console.log(resp)
      return resp.data.filter((snippet: SnippetModel) => snippet.contentSubType === subtype)
    } catch (error: any) {
      if (error.response) {
        throw new Error(`Failed to fetch data: "${error.response.data}"`)
      }
      throw new Error(`Failed to fetch data: ${error}`)
    }
  }
}
