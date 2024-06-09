import type { SnippetModel } from '@/models/SnippetModel'
import { BaseClient } from './BaseClient'

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
}
